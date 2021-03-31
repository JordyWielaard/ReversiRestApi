using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public class SpelerRanking
    {
        public string Guid { get; set; }
        public string Naam { get; set; }
        public int AantalGewonnen { get; set; }
        public int AantalVerloren { get; set; }
        public int AantalGelijk { get; set; }
    }
}
