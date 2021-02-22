using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public interface ISpel
    {
        int ID { get; set; }
        string Omschrijving { get; set; }

        //het unieke token van het spel
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
        Kleur[,] Bord { get; set; }
        Kleur AandeBeurt { get; set; }
        bool Pas();
        bool Afgelopen();

        //welke kleur het meest voorkomend op het spelbord
        Kleur OverwegendeKleur();

        //controle of op een bepaalde positie een zet mogelijk is
        bool ZetMogelijk(int rijZet, int kolomZet);
        bool DoeZet(int rijZet, int kolomZet);
    }
}
