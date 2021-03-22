﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ReversiRestApi.Model
{
    public class SpelRepository : ISpelRepository
    {
        // Lijst met tijdelijke spellen
        public List<Spel> Spellen { get; set; }
        public SpelRepository()
        {
            Spel spel1 = new Spel();
            Spel spel2 = new Spel();
            Spel spel3 = new Spel();

            spel1.Token = "joejoe";
            spel1.Speler1Token = "abcdef";
            spel1.Speler2Token = "abcdefg";
            spel1.Omschrijving = "Potje snel reveri, dus niet lang nadenken";
            spel2.Token = "joe";
            spel2.Speler1Token = "ghijkl";
            spel2.Speler2Token = "mnopqr";
            spel2.Omschrijving = "Ik zoek een gevorderde tegenspeler!";
            spel3.Token = "jo";
            spel3.Speler1Token = "stuvwx";
            spel3.Omschrijving = "Na dit spel wil ik er nog een paar spelen tegen zelfde tegenstander";

            Spellen = new List<Spel> { spel1, spel2, spel3 };
        }

        public void AddSpel(Spel spel)
        {
            Spellen.Add(spel);
        }

        public void JoinSpel(JoinGame data)
        {

        }

        public void DeleteSpel(string spelToken) { }

        public List<Spel> GetSpellen()
        {
            return Spellen;
        }

        public Spel GetSpel(string spelToken)
        {
            return (Spel)(from value in Spellen where spelToken == value.Speler1Token select value);
        }

    }
}