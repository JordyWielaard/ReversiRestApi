using ReversiRestApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public class Spel : ISpel
    {
        private Dictionary<string, Kleur> PlayerMapping = new Dictionary<string, Kleur>();
        public int ID { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        private string _speler1Token;
        public string Speler1Token
        {
            get => _speler1Token;
            set
            {
                _speler1Token = value;
                PlayerMapping.TryAdd(Speler1Token, Kleur.Zwart);
            }
        }
        private string _speler2Token;
        public string Speler2Token
        {
            get => _speler2Token;
            set
            {
                _speler2Token = value;
                PlayerMapping.TryAdd(Speler2Token, Kleur.Wit);
            }
        }
        public Kleur[,] Bord { get; set; }
        public Kleur AandeBeurt { get; set; }
        public List<Coordinaten> Coordinaten { get; set; }
        public string Winner { get; private set; }
        public bool Finished { get; private set; }

        public Spel()
        {
            Coordinaten = new List<Coordinaten>();
            Bord = new Kleur[8, 8];
            Bord[3, 3] = Kleur.Wit;
            Bord[3, 4] = Kleur.Zwart;
            Bord[4, 3] = Kleur.Zwart;
            Bord[4, 4] = Kleur.Wit;
        }
        public Kleur GetPlayerColour(string playerToken)
        {
            try
            {
                Kleur playerColour;

                PlayerMapping.TryGetValue(playerToken, out playerColour);

                return playerColour;
            }
            catch (Exception _)
            {
                return Kleur.Geen;
            }

        }
        public bool Surrender(string playerID)
        {
            if (playerID.Equals(Speler1Token))
            {
                Winner = Speler2Token;
                Finished = true;
                return true;

            }
            else if (playerID.Equals(Speler2Token))
            {
                Winner = Speler1Token;
                Finished = true;
                return true;
            }
            else
            {
                //Coulndt find player to surrender
                return false;
            }
        }
        public bool Afgelopen()
        {
            for (int col = 0; col < Bord.GetLength(0); col++)
            {
                for (int row = 0; row < Bord.GetLength(1); row++)
                {
                    if (ZetMogelijk(row, col))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool DoeZet(int rijZet, int kolomZet)
        {
            if (ZetMogelijk(rijZet, kolomZet))
            {
                VeranderKleurTile(AandeBeurt);
                Bord[rijZet, kolomZet] = AandeBeurt;
                if (AandeBeurt == Kleur.Zwart)
                {
                    AandeBeurt = Kleur.Wit;
                }
                else
                {
                    AandeBeurt = Kleur.Zwart;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void VeranderKleurTile(Kleur aanZet)
        {
            foreach (var item in Coordinaten)
            {
                if (aanZet == Kleur.Wit)
                {
                    Bord[item.X, item.Y] = Kleur.Wit;
                }
                else if (aanZet == Kleur.Zwart)
                {
                    Bord[item.X, item.Y] = Kleur.Zwart;
                }
            }
            Coordinaten.Clear();
        }

        public Kleur OverwegendeKleur()
        {
            int countBlack = 0;
            int countWhite = 0;
            int countGeen = 0;
            for (int col = 0; col < Bord.GetLength(0); col++)
            {
                for (int row = 0; row < Bord.GetLength(1); row++)
                {
                    if (Bord[col, row].Equals(Kleur.Zwart))
                    {
                        countBlack++;
                    }
                    else if (
                        Bord[col, row].Equals(Kleur.Wit))
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

        public bool CheckPieceDirection(Direction direction, Kleur kleur, int row, int col)
        {
            switch (direction)
            {
                case Direction.NorthWest:
                    for (int i = row, j = col, k = 0; k < Math.Min(0 + row, 0 + col); i--, j--, k++)
                    {                    
                        if (Bord[i, j] == kleur)
                        {
                            return true;
                        }
                        else if(Bord[i, j] == Kleur.Geen)
                        {
                            break;
                        }
                        Coordinaten.Add(new Coordinaten(i, j));
                    }
                    break;
                case Direction.North:
                    for (int i = row; i >= 0; i--)
                    {
                        if (Bord[i, col] == kleur)
                        {
                            return true;
                        }
                        else if (Bord[i, col] == Kleur.Geen)
                        {
                            break;
                        }
                        Coordinaten.Add(new Coordinaten(i, col));
                    }
                    break;
                case Direction.NorthEast:
                    for (int i = row, j = col, k = 0; k < Math.Min(0 + row, 8 - col); i--, j++, k++)
                    {
                        if (Bord[i, j] == kleur)
                        {
                            return true;
                        }
                        else if (Bord[i, j] == Kleur.Geen)
                        {
                            break;
                        }
                        Coordinaten.Add(new Coordinaten(i, j));
                    }
                    break;
                case Direction.West:
                    for (int i = col; i >= 0; i--)
                    {
                        if (Bord[row, i] == kleur)
                        {
                            return true;
                        }
                        else if (Bord[row, i] == Kleur.Geen)
                        {
                            break;
                        }
                        Coordinaten.Add(new Coordinaten(row, i));
                    }
                    break;
                case Direction.East:
                    for (int i = col; i < 8; i++)
                    {
                        if (Bord[row, i] == kleur)
                        {
                            return true;
                        }
                        else if (Bord[i, col] == Kleur.Geen)
                        {
                            break;
                        }
                        Coordinaten.Add(new Coordinaten(i, col));
                    }
                    break;
                case Direction.SouthWest:
                    for (int i = row, j = col, k = 0; k < Math.Min(8 - row, 0 + col); i++, j--, k++)
                    {
                        if (Bord[i, j] == kleur)
                        {
                            return true;
                        }
                        else if (Bord[i, j] == Kleur.Geen)
                        {
                            break;
                        }
                        Coordinaten.Add(new Coordinaten(i, j));
                    }
                    break;
                case Direction.South:
                    for (int i = row; i < 8; i++)
                    {
                        if (Bord[i, col] == kleur)
                        {
                            return true;
                        }
                        else if (Bord[i, col] == Kleur.Geen)
                        {
                            break;
                        }
                        Coordinaten.Add(new Coordinaten(i, col));
                    }
                    break;
                case Direction.SouthEast:
                    for (int i = row, j = col, k = 0; k < Math.Min(8 - row, 8 - col); i++, j++, k++)
                    {
                        if (Bord[i, j] == kleur)
                        {
                            return true;
                        }
                        else if (Bord[i, j] == Kleur.Geen)
                        {
                            break;
                        }
                        Coordinaten.Add(new Coordinaten(i, j));
                    }
                    break;
            }
            Coordinaten.Clear();
            return false;
        }

        public bool ZetMogelijk(int rijZet, int kolomZet)
        {
            int direction = 0;
            int startrow = rijZet - 1;
            int startcol = kolomZet - 1;

            if (rijZet < 8 && kolomZet < 8 && rijZet > -1 && kolomZet > -1 && Bord[rijZet, kolomZet].Equals(Kleur.Geen))
            {
                for (int i = 0; i < 3; i++, startrow++)
                {
                    if (startrow < 8 && startrow > -1)
                    {
                        for (int j = 0; j < 3; startcol++, j++, direction++)
                        { 
                            if (startcol < 8 && startcol > -1 && direction != 4)
                            {
                                if ((AandeBeurt.Equals(Kleur.Zwart) && Bord[startrow, startcol].Equals(Kleur.Wit)) || (AandeBeurt.Equals(Kleur.Wit) && Bord[startrow, startcol].Equals(Kleur.Zwart)))
                                {
                                    if (CheckPieceDirection((Direction)direction, AandeBeurt, startrow, startcol))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        startcol -= 3;
                    }
                    else
                    {
                        direction += 3;
                    }
                }
                return false;
            }
            return false;
        }
    }
}
