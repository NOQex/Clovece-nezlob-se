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
        private List<Button> herniPole = new List<Button>();
        private Hrac hrac = new Hrac();

        public Form1()
        {
            InitializeComponent();
            VytvorHerniPlochu();
        }

        private void VytvorHerniPlochu()
        {
            int x = 20;
            for (int i = 0; i < 20; i++) //vytvoří podle parametrů políčko
            {
                Button btn = new Button();
                btn.Width = 40;
                btn.Height = 40;
                btn.Left = x;
                btn.Top = 100;
                btn.Text = (i + 1).ToString();
                this.Controls.Add(btn);
                herniPole.Add(btn); //uloží ho na formulář
                x += 45;
            }
        }


        private void btnHodKostkou_Click(object sender, EventArgs e)
        {
            int cislo = random.Next(1, 7);
            lblkostka.Text = "Hodil jsi: " + cislo;


            if (!hrac.JeNaDesce)//nasadí se do hřiště
            {
                if (cislo == 6)
                {
                    hrac.Nasadit();
                    AktualizujBarvy();
                }
            }
            else
            {
                hrac.Posunout(cislo);//posune se v poli
                AktualizujBarvy();
            }
        }

        private void AktualizujBarvy()
        {
            foreach (var b in herniPole) //vyresetuje barvu
            {
                b.BackColor = SystemColors.Control;
            }

            if (hrac.JeNaDesce && hrac.Pozice < herniPole.Count) //nastaví barvu políčka kde stojí fygurka
            {
                herniPole[hrac.Pozice].BackColor = Color.Red;
            }

        }


    }

}
