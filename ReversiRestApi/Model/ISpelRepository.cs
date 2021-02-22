using ReversiRestApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public interface ISpelRepository
    {
        void AddSpel(Spel spel);
        public List<Spel> GetSpellen();
        Spel GetSpel(string spelToken);
    }
}
