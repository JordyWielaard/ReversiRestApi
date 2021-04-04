using ReversiRestApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public class Spel : ISpel
    {
        public int ID { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
        public Kleur[,] Bord { get; set; }
        public Kleur AandeBeurt { get; set; }
        public string Winnaar { get; set; }
        public bool Afgelopen { get; set; }

        public Spel()
        {
            Bord = new Kleur[8, 8];
            Bord[3, 3] = Kleur.Wit;
            Bord[3, 4] = Kleur.Zwart;
            Bord[4, 3] = Kleur.Zwart;
            Bord[4, 4] = Kleur.Wit;
            AandeBeurt = Kleur.Zwart;
            Afgelopen = false;

        }
        //Haal het aantal Pieces op dat een speler heeft
        public int SpelerPieces(Kleur kleur)
        {
            int aantal = 0;
            for (int opzij = 0; opzij < Bord.GetLength(0); opzij++)
            {
                for (int omlaag = 0; omlaag < Bord.GetLength(1); omlaag++)
                {
                    if (kleur == Kleur.Wit)
                    {
                        if (Bord[opzij, omlaag] == kleur || Bord[opzij, omlaag] == Kleur.NieuwWit)
                        {
                            aantal++;
                        }

                    }
                    else
                    {
                        if (Bord[opzij, omlaag] == kleur || Bord[opzij, omlaag] == Kleur.NieuwZwart)
                        {
                            aantal++;
                        }
                    }
                }
            }
            return aantal;
        }

        //Maak een lijst van posities waar een fiche moet worden veranderd
        public List<Positie> OmTeKerenFichesInRichting(Direction richting, Kleur kleurAandeBeurt, Positie positie)
        {
            List<Positie> voorlopigeLijst = new List<Positie>();

            //zet de positie om te checken
            var buurPositie = CheckRichting(richting,positie);
            //kijk of het niet null is en of er op die positie een fiche van een tegenstander staat
            while (buurPositie != null && Bord[buurPositie.Opzij, buurPositie.Omlaag] == Tegenstander(kleurAandeBeurt))
            {
                voorlopigeLijst.Add(buurPositie);
                //ga naar de volgende positie
                buurPositie = CheckRichting(richting, buurPositie);
            }
            //als er een andere kleur wordt gevonden wordt er gekeken of het de kleur van de speler die aan de beurt is is, zo ja return de list zo nee return een lege list
            if (buurPositie != null && Bord[buurPositie.Opzij, buurPositie.Omlaag] == kleurAandeBeurt)
            {
                return voorlopigeLijst;
            }

            return new List<Positie>();
        }

        //Kijken wat de kleur van de regenstander en en return deze
        public Kleur Tegenstander(Kleur aandeBeurt)
        {
            if (AandeBeurt == Kleur.Wit)
            {
                return Kleur.Zwart;
            }
            else if (aandeBeurt == Kleur.Zwart)
            {
                return Kleur.Wit;
            }
            else
            {
                return Kleur.Geen;
            }
        }

        //Check in de richting die mee gegeven wordt als dit van het bord is return null anders return een nieuwe positie met die x en y
        public Positie CheckRichting(Direction richting, Positie huidigePositie)
        {
            switch (richting)
            {
                case Direction.NorthWest:
                    if (huidigePositie.Opzij == 0 || huidigePositie.Omlaag == 0)
                    {
                        return null;
                    }
                    return new Positie(huidigePositie.Opzij -1, huidigePositie.Omlaag - 1);
                case Direction.North:
                    if (huidigePositie.Omlaag == 0)
                    {
                        return null;
                    }
                    return new Positie(huidigePositie.Opzij, huidigePositie.Omlaag-1);
                case Direction.NorthEast:
                    if (huidigePositie.Opzij == 7 || huidigePositie.Omlaag == 0)
                    {
                        return null;
                    }
                    return new Positie(huidigePositie.Opzij+1, huidigePositie.Omlaag-1);
                case Direction.West:
                    if (huidigePositie.Opzij == 0)
                    {
                        return null;
                    }
                    return new Positie(huidigePositie.Opzij - 1, huidigePositie.Omlaag);
                case Direction.East:
                    if (huidigePositie.Opzij == 7)
                    {
                        return null;
                    }
                    return new Positie(huidigePositie.Opzij + 1, huidigePositie.Omlaag);
                case Direction.SouthWest:
                    if (huidigePositie.Opzij == 0 || huidigePositie.Omlaag == 7)
                    {
                        return null;
                    }
                    return new Positie(huidigePositie.Opzij - 1, huidigePositie.Omlaag + 1);
                case Direction.South:
                    if (huidigePositie.Omlaag == 7)
                    {
                        return null;
                    }
                    return new Positie(huidigePositie.Opzij, huidigePositie.Omlaag + 1);
                case Direction.SouthEast:
                    if (huidigePositie.Opzij == 7 || huidigePositie.Omlaag == 7)
                    {
                        return null;
                    }
                    return new Positie(huidigePositie.Opzij + 1, huidigePositie.Omlaag + 1);
            }
            return null;
        }

        //Check of de speler aan de beurt is
        public bool CheckAandeBeurt(string spelerToken)
        {
            if (AandeBeurt == Kleur.Wit && Speler2Token == spelerToken)
            {
                return true;
            }
            else if (AandeBeurt == Kleur.Zwart && Speler1Token == spelerToken)
            {
                return true;
            }
            return false;
        }

        //Geeft het spel op voor de speler en zet de tegenstander als winnaar en het spel op afgelopen
        public bool Surrender(string playerID)
        {
            if (playerID == Speler1Token)
            {
                Winnaar = Speler2Token;
                Afgelopen = true;
                return true;

            }
            else if (playerID == Speler2Token)
            {
                Winnaar = Speler1Token;
                Afgelopen = true;
                return true;
            }
            else
            {
                //verkeerde token
                return false;
            }
        }
        //Kijkt of het spel is afgelopen zo ja return true zo nee return false
        public bool SpelAfgelopen()
        {
            for (int opzij = 0; opzij < Bord.GetLength(0); opzij++)
            {
                for (int omlaag = 0; omlaag < Bord.GetLength(1); omlaag++)
                {
                    if (ZetMogelijk(opzij, omlaag).Count() > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //Als de zet kan wordt deze gedaan en wordt er true gereturned en de andere speler is aan de beurt anders return false
        public bool DoeZet(int omlaag, int opzij)
        {
            if (ZetMogelijk(omlaag, opzij).Count() > 0)
            {
                foreach (var item in ZetMogelijk(omlaag, opzij))
                {
                    //verander de fiches naar de juiste kleur in de juiste richtingen
                    VeranderKleurTile(AandeBeurt, OmTeKerenFichesInRichting(item, AandeBeurt, new Positie(omlaag, opzij)));
                }
                    
                Bord[omlaag, opzij] = AandeBeurt;
                return Pas();
            }
            else
            {
                return false;
            }
        }

        //Update fiche van nieuw fiche naar normaal fiche voor animatie in client
        public void UpdateNieuweFiches()
        {
            for (int opzij = 0; opzij < Bord.GetLength(0); opzij++)
            {
                for (int omlaag = 0; omlaag < Bord.GetLength(1); omlaag++)
                {
                    if (Bord[opzij, omlaag] == Kleur.NieuwWit)
                    {
                        Bord[opzij, omlaag] = Kleur.Wit;
                    }
                    else if(Bord[opzij, omlaag] == Kleur.NieuwZwart)
                    {
                        Bord[opzij, omlaag] = Kleur.Zwart;
                    }
                }
            }
        }

        //Verander de kleur van een fiche op een bepaalde positie
        public void VeranderKleurTile(Kleur aanZet, List<Positie> posities)
        {
            foreach (var item in posities)
            {
                if (aanZet == Kleur.Wit)
                {
                    Bord[item.Opzij, item.Omlaag] = Kleur.NieuwWit;
                }
                else
                {
                    Bord[item.Opzij, item.Omlaag] = Kleur.NieuwZwart;
                }              
            }
        }

        //check wie er meer fiches heeft
        public Kleur OverwegendeKleur()
        {
            int countBlack = 0;
            int countWhite = 0;
            int countGeen = 0;
            for (int omlaag = 0; omlaag < Bord.GetLength(0); omlaag++)
            {
                for (int opzij = 0; opzij < Bord.GetLength(1); opzij++)
                {
                    if (Bord[omlaag, opzij].Equals(Kleur.Zwart) || Bord[omlaag, opzij].Equals(Kleur.NieuwZwart))
                    {
                        countBlack++;
                    }
                    else if (Bord[omlaag, opzij].Equals(Kleur.Wit) || Bord[omlaag, opzij].Equals(Kleur.NieuwWit))
                    {
                        countWhite++;
                    }
                    else
                    {
                        countGeen++;
                    }
                }
            }

            if (countBlack > countWhite || countBlack > countGeen)
            {
                return Kleur.Zwart;
            }
            else if (countWhite > countBlack || countWhite > countGeen)
            {
                return Kleur.Wit;
            }
            else
            {
                return Kleur.Geen;
            }
        }

        //Zet de winnaar van het spel
        public void WinnaarSpel(Kleur kleur)
        {
            if (kleur.Equals(Kleur.Wit))
            {
                Winnaar = Speler2Token;
            }
            else if(kleur.Equals(Kleur.Zwart))
            {
                Winnaar = Speler1Token;
            }
            else
            {
                Winnaar = "Gelijkspel";
            }
        }

        //Pas de zet naar de andere speler
        public bool Pas()
        {
            if (AandeBeurt.Equals(Kleur.Wit))
            {
                AandeBeurt = Kleur.Zwart;
                return true;
            }
            else
            {
                AandeBeurt = Kleur.Wit;
                return true;
            }
        }

        //Kijk of een zet mogelijk is in een direction en return een lijst met directions waar een zet mogelijk is
        public List<Direction> ZetMogelijk(int opzij, int omlaag)
        {
            var positie = new Positie(opzij, omlaag);
            List<Direction> posibleDirections = new List<Direction>();
            if (omlaag < 8 && opzij < 8 && omlaag > -1 && opzij > -1 && Bord[opzij, omlaag].Equals(Kleur.Geen))
            {
                for (int i = 0; i < 8; i++)
                {
                    if (OmTeKerenFichesInRichting((Direction)i, AandeBeurt, positie).Count > 0)
                    {
                        posibleDirections.Add((Direction)i);
                    }
                }
            }
            return posibleDirections;
        }
    }
}
