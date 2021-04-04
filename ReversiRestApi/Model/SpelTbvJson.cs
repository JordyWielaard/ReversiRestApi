using Newtonsoft.Json;
using System;
namespace ReversiRestApi.Model
{
    public class SpelTbvJson
    {
        public int ID { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
        public string Bord { get; set; }
        public Kleur AandeBeurt { get; set; }
        public bool Afgelopen { get; set; }
        public string Winnaar { get; set; }

        //Deze class zet een spel om naar JSON
        public SpelTbvJson(Spel spel)
        {
            ID = spel.ID;
            Omschrijving = spel.Omschrijving;
            Token = spel.Token;
            Speler1Token = spel.Speler1Token;
            Speler2Token = spel.Speler2Token;
            Bord = JsonConvert.SerializeObject(spel.Bord);
            AandeBeurt = spel.AandeBeurt;
            Afgelopen = spel.Afgelopen;
            Winnaar = spel.Winnaar;        
        }
    }
}