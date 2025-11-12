using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hra_člověče_nezlob_se
{
    internal class Hrac
    {
        public int Pozice { get; set; } = -1; //-1 znamená že figurka není v herním poli
        public bool JeNaDesce => Pozice >= 0;

        public void Nasadit()
        {
            Pozice = 0;
        }

        public void Posunout(int pocetPoli)
        {
            if (JeNaDesce)
            {
                Pozice += pocetPoli;
            }

        }
    }
}
