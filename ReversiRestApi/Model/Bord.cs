using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public class Bord
    {
        public string Token { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public string Kleur { get; set; }
    }
}
