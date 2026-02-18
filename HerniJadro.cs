using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hra_člověče_nezlob_se
{
    // Enum pro sledování stavu, kde se figurka nachází
    public enum StavFigurky
    {
        Doma,       // Čeká na nasazení (potřebuje 6)
        NaDesce,    // Běhá po okruhu
        VCili       // Je v bezpečí v cílovém domečku
    }

    public class Hrac
    {
        public int ID { get; set; } // ID hráče (1, 2, 3, 4) - uložené v PoleDesky
        public string Jmeno { get; set; }
        public Color Barva { get; set; }
        public int StartovniPozice { get; set; } // Kde hráč začíná (0, 10, 20, 30)
        public StavFigurky Stav { get; set; } = StavFigurky.Doma;
        public int PoziceVCili { get; set; } = -1; // 0 až 3 (pozice uvnitř cílového domečku)
    }

    public class HerniJadro
    {
        public const int PocetPoli = 40;
        private int[] PoleDesky = new int[PocetPoli];
        public int[,] CiloveDomecky = new int[4, 4];

        public List<Hrac> Hraci { get; private set; }
        public int HracNaRadeIndex { get; private set; } = 0;

        public HerniJadro()
        {
            Hraci = new List<Hrac>();
            InicializujHru();
        }

        private void InicializujHru()
        {
            //přidání jednotlivých hráčů
            Hraci.Add(new Hrac { ID = 1, Jmeno = "Červený", Barva = Color.Red, StartovniPozice = 0 });
            Hraci.Add(new Hrac { ID = 2, Jmeno = "Zelený", Barva = Color.Green, StartovniPozice = 10 });
            Hraci.Add(new Hrac { ID = 3, Jmeno = "Modrý", Barva = Color.Blue, StartovniPozice = 20 });
            Hraci.Add(new Hrac { ID = 4, Jmeno = "Žlutý", Barva = Color.Yellow, StartovniPozice = 30 });
        }

        public int ZjistiAktualniPoziciNaDesce(Hrac hrac)
        {
            //projde polem a najde kde se figurka hráče nachází
            for (int i = 0; i < PocetPoli; i++)
            {
                if (PoleDesky[i] == hrac.ID)
                {
                    return i;
                }
            }
            return -1; //figurka není na desce
        }

        public void ProvestTah(int hodKostkou)
        {
            var hrac = Hraci[HracNaRadeIndex];

            if (hrac.Stav == StavFigurky.Doma)
            {
                if (hodKostkou == 6)
                {
                    // Nasadíme figurku na start
                    // Nejdřív vyhodíme někoho, kdo tam případně stojí
                    VyhodHraceNaPozici(hrac.StartovniPozice);

                    PoleDesky[hrac.StartovniPozice] = hrac.ID;
                    hrac.Stav = StavFigurky.NaDesce;
                }
            }
            else if (hrac.Stav == StavFigurky.NaDesce)
            {
                int staraPozice = ZjistiAktualniPoziciNaDesce(hrac);
                int novaPozice = staraPozice + hodKostkou;

                // Kontrola, zda nejdeme do domečku
                // Musíme zjistit, jestli přejel svůj start (cílová rovinka je před startovním polem)
                // Pro červeného (start 0) je cíl "před nulou" (tedy po 39)

                int vstupDoDomecku = (hrac.StartovniPozice == 0) ? 40 : hrac.StartovniPozice;

                // Specifická logika pro přechod přes konec pole (např. z 38 na 2)
                // Pokud je start 0, musíme to ošetřit jinak, protože indexy rostou 38, 39, 0...
                bool jdeDoCile = false;
                int poziceVDomecku = -1;

                if (hrac.StartovniPozice == 0)
                {
                    // Pro červeného: pokud je na 35+ a hodí tolik, že přesáhne 39
                    if (staraPozice >= 34 && (staraPozice + hodKostkou) >= 40)
                    {
                        jdeDoCile = true;
                        poziceVDomecku = (staraPozice + hodKostkou) - 40;
                    }
                }
                else
                {
                    // Pro ostatní: pokud by nová pozice byla >= start a stará byla < start
                    if (novaPozice >= hrac.StartovniPozice && staraPozice < hrac.StartovniPozice)
                    {
                        jdeDoCile = true;
                        poziceVDomecku = novaPozice - hrac.StartovniPozice;
                    }
                }

                if (jdeDoCile)
                {
                    if (poziceVDomecku < 4) // Vejde se do domečku?
                    {
                        if (CiloveDomecky[hrac.ID - 1, poziceVDomecku] == 0) // Je tam volno?
                        {
                            PoleDesky[staraPozice] = 0; // Smaž z desky
                            CiloveDomecky[hrac.ID - 1, poziceVDomecku] = hrac.ID; // Dej do cíle
                            hrac.Stav = StavFigurky.VCili;
                            hrac.PoziceVCili = poziceVDomecku;
                        }
                    }
                }
                else
                {
                    // Normální pohyb po desce
                    novaPozice = novaPozice % PocetPoli; // Zacyklení 39 -> 0

                    VyhodHraceNaPozici(novaPozice); // Vyhazování

                    PoleDesky[staraPozice] = 0;
                    PoleDesky[novaPozice] = hrac.ID;
                }
            }
            else if (hrac.Stav == StavFigurky.VCili)
            {
                // Zde by mohla být logika pro posouvání v rámci cílového domečku
            }

            // Předání tahu (pokud padla 6, hraje znovu - volitelné pravidlo, zatím necháme střídání)
            if (hodKostkou != 6)
            {
                HracNaRadeIndex = (HracNaRadeIndex + 1) % Hraci.Count;
            }
        }

        private void VyhodHraceNaPozici(int pozice)
        {
            int idSoupere = PoleDesky[pozice];
            if (idSoupere != 0)
            {
                var vyhozeny = Hraci.First(h => h.ID == idSoupere);
                vyhozeny.Stav = StavFigurky.Doma; // Jde zpátky "do krabice"
                // PoleDesky se přepíše novým hráčem v hlavní metodě
            }
        }

        //metoda která vykreslí aktuální stav
        public int[] ZiskejStavPole()
        {
            return (int[])PoleDesky.Clone();
        }

        public int[] ZiskejStavCile(int hracID)
        {
            int[] domecek = new int[4];
            for (int i = 0; i < 4; i++) domecek[i] = CiloveDomecky[hracID - 1, i];
            return domecek;
        }

        public Color ZiskejBarvuHrace(int id)
        {
            return Hraci.First(h => h.ID == id).Barva;
        }
    }
}
