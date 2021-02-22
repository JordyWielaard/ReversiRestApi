using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public class Surrender
    {
        public string gameToken { get; set; }
        public string playerToken { get; set; }
    }
}
