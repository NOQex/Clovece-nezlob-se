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
        private Random random = new Random();
        private Point[] herniPole;   // pole souřadnic jednotlivých políček
        private PictureBox figurka;
        private Hrac hrac = new Hrac();

        public Form1()
        {
            InitializeComponent();
            VytvorHerniPlochu();
        }

        private void VytvorHerniPlochu()
        {          
            herniPole = new Point[20];

            int x = 20;
            for (int i = 0; i < 20; i++)
            {
                herniPole[i] = new Point(x, 100); // X a Y pozice pro figurku
                x += 45;
            }

            figurka = new PictureBox();
            figurka.Width = 40;
            figurka.Height = 40;
            figurka.BackColor = Color.Red;
            figurka.Visible = false;
            this.Controls.Add(figurka);      
        }


        private void btnHodKostkou_Click(object sender, EventArgs e)
        {
            int cislo = random.Next(1, 7);
            lblkostka.Text = "Hodil jsi: " + cislo;

            if (!hrac.JeNaDesce)
            {
                if (cislo == 6)
                {
                    hrac.Nasadit();
                    AktualizujFigurku();
                }
            }
            else
            {
                hrac.Posunout(cislo);
                AktualizujFigurku();
            }
        }


        private void AktualizujFigurku()
        {
            if (hrac.JeNaDesce && hrac.Pozice < herniPole.Length)
            {
                figurka.Visible = true;
                figurka.Location = herniPole[hrac.Pozice];
            }
            else
            {
                figurka.Visible = false;
            }
        }



    }

}
