using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public class Positie
    {
        public int Opzij { get; set; }
        public int Omlaag { get; set; }

        public Positie(int x, int y)
        {
            Opzij = x;
            Omlaag = y;
        }
    }
}
