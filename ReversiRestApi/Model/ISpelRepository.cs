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
        void JoinSpel(JoinGame data);
        public List<Spel> GetSpellen();
        Spel GetSpel(string spelToken);
        void DeleteSpel(string spelToken);
        void UpdateSpel(Spel spel);
        void OpgevenSpel(Spel spel);
        void AddPieceHistoryteSpel(string spelToken, int aantal1, int aantal2);
        List<int> GetPieceHistory(string spelToken, string spelerToken);
    }
}
