using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Hra_člověče_nezlob_se
{
    public partial class Form1 : Form
    {
        private HerniJadro hra;
        private Random kostka;
        private Point[] mapaCesty; // pole pro trasu herní desky

        public Form1()
        {
            InitializeComponent();
            hra = new HerniJadro(); // uložení herního jádra
            kostka = new Random();
            this.DoubleBuffered = true; // dvojité ukládání do paměti, čímž se zamezí problikávání obrazu při překreslování

            InicializujMapuCesty(); // naplní pole mapaCesty přesnými souřadnicemi
            AktualizujInfoLabel(); // vypíše text o tom, kdo je na řadě
        }

        private void InicializujMapuCesty() // nastavuje pevné X a Y souřadnice pro všech 40 políček
        {
            mapaCesty = new Point[40];

            // 1. Úsek: vlevo vodorovně
            mapaCesty[0] = new Point(0, 4); // nastavuje 1. políčko na X=0, Y=4 v mřížce
            mapaCesty[1] = new Point(1, 4);
            mapaCesty[2] = new Point(2, 4);
            mapaCesty[3] = new Point(3, 4);
            mapaCesty[4] = new Point(4, 4);

            // 2. Úsek: nahoru
            mapaCesty[5] = new Point(4, 3);
            mapaCesty[6] = new Point(4, 2);
            mapaCesty[7] = new Point(4, 1);
            mapaCesty[8] = new Point(4, 0);

            // 3. Úsek: horní oblouk
            mapaCesty[9] = new Point(5, 0);
            mapaCesty[10] = new Point(6, 0);

            // 4. Úsek: dolů
            mapaCesty[11] = new Point(6, 1);
            mapaCesty[12] = new Point(6, 2);
            mapaCesty[13] = new Point(6, 3);
            mapaCesty[14] = new Point(6, 4);

            // 5. Úsek: doprava
            mapaCesty[15] = new Point(7, 4);
            mapaCesty[16] = new Point(8, 4);
            mapaCesty[17] = new Point(9, 4);
            mapaCesty[18] = new Point(10, 4);

            // 6. Úsek: pravý oblouk
            mapaCesty[19] = new Point(10, 5);
            mapaCesty[20] = new Point(10, 6);

            // 7. Úsek: doleva
            mapaCesty[21] = new Point(9, 6);
            mapaCesty[22] = new Point(8, 6);
            mapaCesty[23] = new Point(7, 6);
            mapaCesty[24] = new Point(6, 6);

            // 8. Úsek: dolů
            mapaCesty[25] = new Point(6, 7);
            mapaCesty[26] = new Point(6, 8);
            mapaCesty[27] = new Point(6, 9);
            mapaCesty[28] = new Point(6, 10);

            // 9. Úsek: spodní oblouk
            mapaCesty[29] = new Point(5, 10);
            mapaCesty[30] = new Point(4, 10);

            // 10. Úsek: nahoru
            mapaCesty[31] = new Point(4, 9);
            mapaCesty[32] = new Point(4, 8);
            mapaCesty[33] = new Point(4, 7);
            mapaCesty[34] = new Point(4, 6);

            // 11. Úsek: doleva
            mapaCesty[35] = new Point(3, 6);
            mapaCesty[36] = new Point(2, 6);
            mapaCesty[37] = new Point(1, 6);
            mapaCesty[38] = new Point(0, 6);

            // 12. Úsek: levý oblouk (konec kola)
            mapaCesty[39] = new Point(0, 5);
        }

        private void btnHodit_Click(object sender, EventArgs e)
        {
            int hod = kostka.Next(1, 7); // generuje náhodné celé číslo od 1 do 6
            var hracNaRade = hra.Hraci[hra.HracNaRadeIndex]; // získá ze seznamu hráčů toho, který je na řadě

            hra.ProvestTah(hod); // předává hernímu jádru co padlo na kostce

            lblInfo.Text = $"{hracNaRade.Jmeno} hodil {hod}. {hra.ZpravaOTahu}\n"; // zobrazí který hráč kolik hodil
            lblInfo.Text += $"Na řadě: {hra.Hraci[hra.HracNaRadeIndex].Jmeno}"; // zobrazí který hráč je na řadě

            pictureBoxDeska.Invalidate(); // vypne překreslování
        }

        private void AktualizujInfoLabel()
        {
            lblInfo.Text = $"Na řadě: {hra.Hraci[hra.HracNaRadeIndex].Jmeno}\nHodit kostkou pro zahájení."; // vypíše, kdo začíná
        }

        private void pictureBoxDeska_Paint(object sender, PaintEventArgs e) // metoda která vykresluje celou grafiku
        {
            Graphics g = e.Graphics; // štětec pro kreslení tvarů
            g.SmoothingMode = SmoothingMode.AntiAlias; // udělá hladké kolečka

            int minRozmer = Math.Min(pictureBoxDeska.Width, pictureBoxDeska.Height); // herní deska se vykreslí jako čtverec
            int velikostGridu = 11; // hrací plocha je rozdělena na síť 11x11
            int velikostPole = (minRozmer / velikostGridu) - 2; // velikost jednoho kolečka v pixelech (rozměr plátna děleno počtem buněk, minus 2 pixely pro mezery)
            int offset = 10; // odsazení od okraje

            for (int i = 0; i < 40; i++)
            {
                VykresliPoleGrid(g, mapaCesty[i].X, mapaCesty[i].Y, velikostPole, offset, Pens.Black, Brushes.White); // vykreslí políčka na daných místech
            }

            VykresliCiloveDomecky(g, velikostPole, offset); // vykreslí cílové domečky

            VykresliStartovniZakladny(g, velikostPole, offset); // vykreslí startovní domečky

            int[] stavPole = hra.ZiskejStavPole(); // z herního jádra si vyžádá pole čísel
            for (int i = 0; i < 40; i++) // kontroluje každé z 40 políček, zda na něm někdo nestojí
            {
                if (stavPole[i] > 0) // zjišťuje jestli je na poli hráč
                {
                    var barvaHrace = hra.Hraci[stavPole[i] - 1].Barva; // pomocí ID hráče (zmenšeného o 1 kvůli indexování od nuly) zjistí jeho konkrétní barvu ze seznamu hráčů
                    Point gridPos = mapaCesty[i]; // získá souřadnice (X,Y) daného políčka z pole mapaCesty
                    VykresliFigurku(g, gridPos.X, gridPos.Y, velikostPole, offset, barvaHrace); // vykreslí figurku
                }
            }
        }

        private void VykresliPoleGrid(Graphics g, int gridX, int gridY, int size, int offset, Pen pen, Brush bg) // kreslení políček
        {
            int x = offset + (gridX * (size + 2)); // vypočítá X pozici v pixelech (odsazení + (pozice v mřížce * šířka buňky s mezerou)).
            int y = offset + (gridY * (size + 2)); // vypočítá Y pozici v pixelech
            g.FillEllipse(bg, x, y, size, size); // vykreslí vyplněný kruh barvou
            g.DrawEllipse(pen, x, y, size, size); // vykreslí obrys kruhu
        }

        private void VykresliFigurku(Graphics g, int gridX, int gridY, int size, int offset, Color barva) // kreslení figurky
        {
            int x = offset + (gridX * (size + 2)); // vypočítá X pozici políčka, kam figurka patří
            int y = offset + (gridY * (size + 2)); // vypočítá Y pozici políčka, kam figurka patří

            int padding = 4; // vnitřní okraj (odsazení 4 pixely)
            using (Brush b = new SolidBrush(barva)) // štětec z předané barvy
            {
                g.FillEllipse(b, x + padding, y + padding, size - (padding * 2), size - (padding * 2)); // vykreslí figurku
            }
            g.DrawEllipse(Pens.Black, x + padding, y + padding, size - (padding * 2), size - (padding * 2)); // vykreslí černý obrys kolem figurky, aby lépe vynikla
        }

        private void VykresliStartovniZakladny(Graphics g, int size, int offset)
        {
            Point[] starty = { new Point(0, 0), new Point(9, 0), new Point(9, 9), new Point(0, 9) }; // pole určující startovní domečky

            for (int i = 0; i < 4; i++) // projde všechny 4 hráče
            {
                var hrac = hra.Hraci[i]; // získá aktuálního hráče
                Point p = starty[i]; // získá souřadnice začátku startovního domečku daného hráče

                int pocetDoma = hra.ZiskejPocetDoma(hrac.ID); // zjistí, kolik figurek má daný hráč v domečku
                int vykresleno = 0;

                for (int row = 0; row < 2; row++) // průchod 2 řádky startovního domečku
                {
                    for (int col = 0; col < 2; col++) // průchod 2 sloupci startovního domečku
                    {
                        int gx = p.X + col; // finální X souřadnice v domečku
                        int gy = p.Y + row; // finální Y souřadnice v domečku

                        using (Pen penBase = new Pen(hrac.Barva, 2)) // vytvoří pero v barvě hráče o tloušťce 2 pixely
                        {
                            VykresliPoleGrid(g, gx, gy, size, offset, penBase, Brushes.WhiteSmoke); // nakreslí políčko startovního domečku se světle šedou výplní a okrajem v barvě hráče
                        }

                        if (vykresleno < pocetDoma) // kontroluje, jestli se má nakreslit ještě další figurku
                        {
                            VykresliFigurku(g, gx, gy, size, offset, hrac.Barva); // vykreslí figurku na dané políčko v startovním domečku
                            vykresleno++;
                        }
                    }
                }
            }
        }

        private void VykresliCiloveDomecky(Graphics g, int size, int offset) // vykreslení cílového domečku
        {
            for (int hracIdx = 0; hracIdx < 4; hracIdx++) // projde všechny 4 hráče
            {
                var hrac = hra.Hraci[hracIdx]; // konkrétní hráč
                int[] stavCile = hra.ZiskejStavCile(hrac.ID); // kde má hráč v cílovém domečku figurku

                for (int pos = 0; pos < 4; pos++) // políčka cílového domečku daného hráče
                {
                    int gx = 0, gy = 0;

                    switch (hracIdx) // určí směr domečku
                    {
                        case 0: // pokud jde o červeného hráče, kreslí se zleva doprava
                            gx = 1 + pos; gy = 5; // nastaví X pozici od 1 do 4. Y pozice je pevně 5
                            break;
                        case 1: // pokud jde o zeleného hráče, kreslí se shora dolů
                            gx = 5; gy = 1 + pos;
                            break;
                        case 2: // pokud jde o modrého hráče, kreslí se zprava doleva
                            gx = 9 - pos; gy = 5;
                            break;
                        case 3: // pokud jde o žlutého hráče, kreslí se zdola nahoru
                            gx = 5; gy = 9 - pos;
                            break;
                    }

                    using (Pen p = new Pen(hrac.Barva, 2)) // vytvoří se pero v barvě aktuálního hráče o tlouštce 2 pixely
                    {
                        VykresliPoleGrid(g, gx, gy, size, offset, p, Brushes.White); // nakreslí políčko cíle s bílým vnitřkem a okrajem v barvě hráče
                    }

                    if (stavCile[pos] > 0) // zkontroluje jestli na daném políčku leží figurka
                    {
                        VykresliFigurku(g, gx, gy, size, offset, hrac.Barva); // vykreslí figurku na políčko
                    }
                }
            }

            VykresliPoleGrid(g, 5, 5, size, offset, Pens.Black, Brushes.Gray); // nakreslí zcela středové políčko, šedě podbarvené (jen pro vzhled)
        }
    }
}