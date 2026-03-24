using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Hra_člověče_nezlob_se
{
    public class Figurka
    {
        public int UjitaVzdalenost { get; set; } = -1; // -1 znamená v startovním domečku, 0-39 je pozice na cestě vůči startu, 40-43 je pozice v cílovém domečku
    }

    public class Hrac
    {
        public int ID { get; set; } // Identifikátor hráče
        public string Jmeno { get; set; } // pro výpis uživateli
        public Color Barva { get; set; }
        public int StartovniPozice { get; set; } // Index políčka (0, 10, 20 nebo 30)
        public Figurka[] Figurky { get; set; }

        public Hrac()
        {
            Figurky = new Figurka[4];
            for (int i = 0; i < 4; i++) Figurky[i] = new Figurka(); // vytvoříme 4 figurky
        }

        public int ZiskejAbsolutniIndex(int vzdalenost)
        {
            if (vzdalenost < 0 || vzdalenost >= HerniJadro.PocetPoli) return -1; // kontrola jestli je figurka na desce
            return (StartovniPozice + vzdalenost) % HerniJadro.PocetPoli;
        }
    }

    public class HerniJadro
    {
        public const int PocetPoli = 40;
        private int[] PoleDesky = new int[PocetPoli]; //určuje počet polí na desce
        public int[,] CiloveDomecky = new int[4, 4];

        public List<Hrac> Hraci { get; private set; } // seznam všech hráčů
        public int HracNaRadeIndex { get; private set; } = 0;
        public string ZpravaOTahu { get; private set; } = "";

        public HerniJadro()
        {
            Hraci = new List<Hrac>();
            InicializujHru();
            AktualizujDesku();
        }

        private void InicializujHru()
        {
            Hraci.Add(new Hrac { ID = 1, Jmeno = "Červený", Barva = Color.Red, StartovniPozice = 0 });
            Hraci.Add(new Hrac { ID = 2, Jmeno = "Zelený", Barva = Color.Green, StartovniPozice = 10 });
            Hraci.Add(new Hrac { ID = 3, Jmeno = "Modrý", Barva = Color.Blue, StartovniPozice = 20 });
            Hraci.Add(new Hrac { ID = 4, Jmeno = "Žlutý", Barva = Color.Yellow, StartovniPozice = 30 });
        }

        public void ProvestTah(int hodKostkou)
        {
            var hrac = Hraci[HracNaRadeIndex]; // zjistí aktuálního hráče podle indexu, kdo je na tahu
            ZpravaOTahu = ""; // vyčistí textovou zprávu o minulém tahu

            var figurkaDoma = hrac.Figurky.FirstOrDefault(f => f.UjitaVzdalenost == -1); // nalezne první figurku daného hráče, jejíž vzdálenost je -1
            bool startObsazenVlastni = hrac.Figurky.Any(f => f.UjitaVzdalenost == 0); // ověření, zda už má tento hráč nějakou vlastní figurku nasazenou přímo na startovním políčku

            if (hodKostkou == 6 && figurkaDoma != null && !startObsazenVlastni) // podmínka pro nasazení
            {
                VyhodHraceNaPozici(hrac.StartovniPozice); // přivolaná metoda která zkontroluje zda na políčku není jiná figurka, kdyžtak ji vyhodí
                figurkaDoma.UjitaVzdalenost = 0;
                AktualizujDesku(); // metoda pro přepočítání pozic pro vykreslení
                ZpravaOTahu = "Nasazena figurka na start.";
            }
            else
            {
                bool tahProveden = false;

                foreach (var figurka in hrac.Figurky.Where(f => f.UjitaVzdalenost >= 0 && f.UjitaVzdalenost < 44)) // aby se neposouvaly figurky v domečku
                {
                    if (ZkusPohnoutFigurkou(hrac, figurka, hodKostkou)) // kontrola jestli se tah může provést
                    {
                        tahProveden = true;
                        break;
                    }
                }

                if (!tahProveden) ZpravaOTahu = "Žádný tah není možný.";
            }

            if (hodKostkou != 6) HracNaRadeIndex = (HracNaRadeIndex + 1) % Hraci.Count; // pokud hráč nehodil 6, předá tah dalšímu hráči
            else ZpravaOTahu += " Hází znovu!";
        }

        private bool ZkusPohnoutFigurkou(Hrac hrac, Figurka f, int hod)
        {
            int novaVzdalenost = f.UjitaVzdalenost + hod;

            if (novaVzdalenost > 43) return false; // nedovolí vyjet s hrací desky
            if (hrac.Figurky.Any(o => o != f && o.UjitaVzdalenost == novaVzdalenost)) return false; // pokud na políčku stojí vlastní figurka, tak ji nevyhodí

            if (novaVzdalenost >= PocetPoli) // pokud je vzdálenost 40 nebo větší, znamená to, že figurka zachází do cílového domečku
            {
                f.UjitaVzdalenost = novaVzdalenost;
                AktualizujDesku();
                ZpravaOTahu = "Figurka došla do cíle!";
                return true;
            }
            else
            {
                int novyAbsolutniIndex = hrac.ZiskejAbsolutniIndex(novaVzdalenost); // zjistí kam se má figurka posunout
                VyhodHraceNaPozici(novyAbsolutniIndex); // zkontroluje pole, a pokud tam stojí nepřítel, tak zavolá funkci pro jeho vyhození

                f.UjitaVzdalenost = novaVzdalenost;
                AktualizujDesku();
                ZpravaOTahu = $"Posun o {hod} polí.";
                return true;
            }
        }

        private void VyhodHraceNaPozici(int absolutniIndex)
        {
            foreach (var souper in Hraci) // projde všechny hráče
            {
                foreach (var figurka in souper.Figurky) // pro každého nalezeného hráče projde jeho 4 figurky
                {
                    if (figurka.UjitaVzdalenost >= 0 && figurka.UjitaVzdalenost < PocetPoli) // zkontroluje jen figurky mimo demečky
                    {
                        if (souper.ZiskejAbsolutniIndex(figurka.UjitaVzdalenost) == absolutniIndex)
                        {
                            figurka.UjitaVzdalenost = -1; // vyhodí soupeřovu figurku
                            ZpravaOTahu += $" Vyhozen hráč {souper.Jmeno}!";
                        }
                    }
                }
            }
        }

        private void AktualizujDesku()
        {
            Array.Clear(PoleDesky, 0, PoleDesky.Length); // vymaže pole desky
            Array.Clear(CiloveDomecky, 0, CiloveDomecky.Length); // vymaže domečky

            foreach (var hrac in Hraci) // projde všechny hráče
            {
                foreach (var figurka in hrac.Figurky) // projde všechny figurky daného hráče
                {
                    if (figurka.UjitaVzdalenost >= 0 && figurka.UjitaVzdalenost < PocetPoli) // pokud je figurka mimo domeček
                    {
                        int index = hrac.ZiskejAbsolutniIndex(figurka.UjitaVzdalenost); // vypočítá na kterém indexu figurka je
                        PoleDesky[index] = hrac.ID;
                    }
                    else if (figurka.UjitaVzdalenost >= PocetPoli && figurka.UjitaVzdalenost < 44) // pokud figurka dosáhla domku
                    {
                        int poziceVDomecku = figurka.UjitaVzdalenost - PocetPoli;
                        CiloveDomecky[hrac.ID - 1, poziceVDomecku] = hrac.ID; // podle ID hráče vyhledá jeho konkrétní řádek a zapíše na něj ID, že se tam nachází
                    }
                }
            }
        }

        public int[] ZiskejStavPole() => (int[])PoleDesky.Clone(); // naclonuje desku aby se omylem neupravila

        public int[] ZiskejStavCile(int hracID)
        {
            int[] domecek = new int[4]; // dočasné pole
            for (int i = 0; i < 4; i++) domecek[i] = CiloveDomecky[hracID - 1, i]; // z pole CiloveDomecky zkopíruje jen ten řádek, který patří danému hráči
            return domecek;
        }

        public int ZiskejPocetDoma(int hracID)
        {
            return Hraci.First(h => h.ID == hracID).Figurky.Count(f => f.UjitaVzdalenost == -1); // najde hráče dle ID a spočítá, kolik jeho figurek má v domečku
        }
    }
}