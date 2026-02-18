using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hra_člověče_nezlob_se
{
    public partial class Form1 : Form
    {
        private HerniJadro hra;
        private Random kostka;

        public Form1()
        {
            InitializeComponent();

            hra = new HerniJadro();
            kostka = new Random();

            //nastavení PictureBoxu pro lepší vykreslování
            this.DoubleBuffered = true;
            AktualizujInfoLabel();
        }

        private void btnHodit_Click(object sender, EventArgs e)
        {

        }

        private void AktualizujInfoLabel()
        {
            lblInfo.Text = $"Na řadě: {hra.Hraci[hra.HracNaRadeIndex].Jmeno}";
        }

        private void pictureBoxDeska_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; //nevypadá to jako minecraft xdd

            //rozměry hracího pole
            int w = pictureBoxDeska.Width;
            int h = pictureBoxDeska.Height;
            int velikostPole = 30;

            int[] stavPole = hra.ZiskejStavPole(); //aktuální stav pole z jádra

            //vykreslování polí a figurek
            for (int i = 0; i < HerniJadro.PocetPoli; i++)
            {
                //převod logického indexu na grafické souřadnice
                Point bod = ZiskejSouradniceProIndex(i, w, h);

                //kreslení prázdných koleček, alaka cestu
                g.DrawEllipse(Pens.Black, bod.X, bod.Y, velikostPole, velikostPole);

                //kontrolujeme, zda je na políčku figurka
                if (stavPole[i] > 0)
                {
                    VykresliFigurku(g, bod.X, bod.Y, velikostPole, hra.ZiskejBarvuHrace(stavPole[i]));
                }
            }
            // --- 2. VYKRESLENÍ STARTŮ (Domečky pro nenasezené figurky) ---
            VykresliStartovniPozice(g, w, h, velikostPole);

            // --- 3. VYKRESLENÍ CÍLOVÝCH DOMEČKŮ (Střed) ---
            VykresliCiloveDomecky(g, w, h, velikostPole);
        }

        private void VykresliStartovniPozice(Graphics g, int w, int h, int vel)
        {
            // Pozice startovních "hnízd" (v rozích)
            Point[] starty = new Point[] {
                new Point(10, 10),           // Červený (vlevo nahoře)
                new Point(w - 60, 10),       // Zelený (vpravo nahoře)
                new Point(w - 60, h - 60),   // Modrý (vpravo dole)
                new Point(10, h - 60)        // Žlutý (vlevo dole)
            };

            for (int i = 0; i < 4; i++)
            {
                var hrac = hra.Hraci[i];
                // Kreslíme čtvereček jako základnu
                g.DrawRectangle(new Pen(hrac.Barva, 2), starty[i].X, starty[i].Y, vel + 10, vel + 10);

                // Pokud je hráč doma (není ve hře ani v cíli), nakreslíme tam figurku
                if (hrac.Stav == StavFigurky.Doma)
                {
                    VykresliFigurku(g, starty[i].X + 5, starty[i].Y + 5, vel, hrac.Barva);
                }
            }
        }

        private void VykresliCiloveDomecky(Graphics g, int w, int h, int vel)
        {
            int odsazeni = 50;
            int krok = (w - (2 * odsazeni)) / 10;
            int stredX = w / 2;
            int stredY = h / 2;

            // Projdeme všechny 4 hráče
            for (int hracIdx = 0; hracIdx < 4; hracIdx++)
            {
                int[] stavCile = hra.ZiskejStavCile(hracIdx + 1);
                Color barvaHrace = hra.ZiskejBarvuHrace(hracIdx + 1);

                for (int pos = 0; pos < 4; pos++)
                {
                    int x = 0, y = 0;

                    // Výpočet souřadnic pro kříž uprostřed
                    // Červený jde zleva doprava, Zelený shora dolů, atd.
                    switch (hracIdx)
                    {
                        case 0: // Červený (zleva)
                            x = odsazeni + krok + (pos * krok);
                            y = odsazeni + (5 * krok);
                            break;
                        case 1: // Zelený (shora)
                            x = odsazeni + (5 * krok);
                            y = odsazeni + krok + (pos * krok);
                            break;
                        case 2: // Modrý (zprava)
                            x = w - odsazeni - krok - (pos * krok); // oprava odsazení
                            y = odsazeni + (5 * krok);
                            break;
                        case 3: // Žlutý (zdola)
                            x = odsazeni + (5 * krok);
                            y = h - odsazeni - krok - vel - (pos * krok);
                            break;
                    }

                    // Kreslení políčka domečku
                    using (Pen p = new Pen(barvaHrace, 2))
                    {
                        g.DrawEllipse(p, x, y, vel, vel);
                    }

                    // Pokud je tam figurka, vykresli ji
                    if (stavCile[pos] > 0)
                    {
                        VykresliFigurku(g, x, y, vel, barvaHrace);
                    }
                }
            }
        }

        private void VykresliFigurku(Graphics g, int x, int y, int vel, Color barva)
        {
            using (Brush b = new SolidBrush(barva))
            {
                g.FillEllipse(b, x + 2, y + 2, vel - 4, vel - 4);
            }
            g.DrawEllipse(Pens.Black, x + 2, y + 2, vel - 4, vel - 4); // Obrys figurky
        }

        private Point ZiskejSouradniceProIndex(int index, int w, int h)
        {
            int odsazeni = 50;
            int sirkaStrany = w - (2 * odsazeni);
            int pocetPoliNaStranu = 10;
            int krok = sirkaStrany / pocetPoliNaStranu;

            int x = 0;
            int y = 0;

            if (index < 10) //horní strana
            {
                x = odsazeni + (index * krok);
                y = odsazeni;
            }
            else if (index < 20) //pravá strana
            {
                x = odsazeni + (10 * krok);
                y = odsazeni + ((index - 10) * krok);
            }
            else if (index < 30) //spodní strana
            {
                x = odsazeni + (10 * krok) - ((index - 20) * krok);
                y = odsazeni + (10 * krok);
            }
            else //levá strana
            {
                x = odsazeni;
                y = odsazeni + (10 * krok) - ((index - 30) * krok);
            }

            return new Point(x, y);
        }
    }

}
