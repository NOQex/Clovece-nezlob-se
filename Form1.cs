using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hra_člověče_nezlob_se
{
    public partial class Form1 : Form
    {
        private List<Label> policka = new List<Label>();
        private int poziceHrace = -1; // -1 = ještě není na poli
        private Random nahoda = new Random();


        public Form1()
        {
            InitializeComponent();
        }

        private void VytvorHraciPole()
        {
            // Vytvoří 20 polí v řadě
            int pocetPolicek = 20;
            int velikost = 30;

            for (int i = 0; i < pocetPolicek; i++)
            {
                Label pole = new Label();
                pole.Size = new Size(velikost, velikost);
                pole.Location = new Point(10 + i * (velikost + 5), 50);
                pole.BorderStyle = BorderStyle.FixedSingle;
                pole.BackColor = Color.White;
                panelHraciPole.Controls.Add(pole);
                policka.Add(pole);
            }
        }

        private void btnHodKostkou_Click(object sender, EventArgs e)
        {
            int hod = nahoda.Next(1, 7);
            lblCisloKostky.Text = $"Padlo: {hod}";

            if (poziceHrace == -1)
            {
                // figurka nastupuje na start
                poziceHrace = 0;
                policka[0].BackColor = Color.Red;
            }
            else
            {
                // pohyb figurky
                int novaPozice = poziceHrace + hod;

                if (novaPozice >= policka.Count)
                    novaPozice = policka.Count - 1; // konec

                policka[poziceHrace].BackColor = Color.White; // staré políčko
                policka[novaPozice].BackColor = Color.Red;    // nové políčko
                poziceHrace = novaPozice;
            }
        }
    }

}
