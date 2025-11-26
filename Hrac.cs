using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hra_člověče_nezlob_se
{
    public class Hrac
    {
        public int Pozice { get; set; } = -1;
        public bool JeNaDesce => Pozice >= 0;

        public void Nasadit()
        {
            Pozice = 0;
        }

        public void Posunout(int pocetPoli)
        {
            if (JeNaDesce)
                Pozice += pocetPoli;
        }
    }

}
