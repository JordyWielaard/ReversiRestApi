using ReversiRestApi.Controllers;
using ReversiRestApi.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.DAL
{
    public class SpelAccessLayer : ISpelRepository
    {
        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=ReversiDbRestApi; Integrated Security=True;";

        //Insert game to database table & insert all cell values into cell table with the game token
        public void AddSpel(Spel spel)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                //INSET querys for game and bord
                string addSpelQuery = "INSERT INTO Games (Token, Speler1Token, Omschrijving, AandeBeurt, Afgelopen) VALUES(@Token, @Speler1Token, @Omschrijving, @AandeBeurt, @Afgelopen)";
                string addBordQuery = "INSERT INTO Cell (Token, Row, Col, Kleur) VALUES (@Token, @Row, @Col, @Kleur)";
               
                SqlCommand sqlCmd = new SqlCommand(addSpelQuery, sqlCon);

                sqlCmd.Parameters.AddWithValue("@Token", spel.Token);
                sqlCmd.Parameters.AddWithValue("@Speler1Token", spel.Speler1Token);
                sqlCmd.Parameters.AddWithValue("@Omschrijving", spel.Omschrijving);
                sqlCmd.Parameters.AddWithValue("@AandeBeurt", spel.AandeBeurt);
                sqlCmd.Parameters.AddWithValue("@Afgelopen", spel.Afgelopen);

                sqlCon.Open();
                sqlCmd.ExecuteNonQuery();             
                
                //Loops over game bord and adds all cells with x and y locations and the colour that occupies the space and the game token
                for (int i = 0; i < spel.Bord.GetLength(0); i++)
                {
                    for (int j = 0; j < spel.Bord.GetLength(1); j++)
                    {
                        SqlCommand sqlCmdAddBord = new SqlCommand(addBordQuery, sqlCon);
                        sqlCmdAddBord.Parameters.AddWithValue("@Token", spel.Token);
                        sqlCmdAddBord.Parameters.AddWithValue("@Row", i);
                        sqlCmdAddBord.Parameters.AddWithValue("@Col", j);
                        sqlCmdAddBord.Parameters.AddWithValue("@Kleur", spel.Bord[i, j]);
                        sqlCmdAddBord.ExecuteNonQuery();
                    }
                }
                
                sqlCon.Close();

            }
        }

        public void JoinSpel(JoinGame data)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                string joinSpelQuery = "UPDATE Games SET Speler2Token = @spelerToken WHERE Token = @gameToken";

                SqlCommand sqlCmd = new SqlCommand(joinSpelQuery, sqlCon);

                sqlCmd.Parameters.AddWithValue("@spelerToken", data.SpelerToken);
                sqlCmd.Parameters.AddWithValue("@gameToken", data.SpelToken);

                sqlCon.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
            }
        }

        //TODO: Add query to get bord from database
        public Spel GetSpel(string spelToken)
        {
            var spel = new Spel();
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Games JOIN Cell ON Games.Token = Cell.Token WHERE Games.Token = @spelToken";

                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@spelToken", spelToken);
                SqlDataReader rdr = sqlCmd.ExecuteReader();

                while (rdr.Read())
                {
                    spel.ID = Convert.ToInt32(rdr["ID"]);
                    spel.Token = Convert.ToString(rdr["Token"]);
                    spel.Speler1Token = Convert.ToString(rdr["Speler1Token"]);
                    spel.Speler2Token = Convert.ToString(rdr["Speler2Token"]);
                    spel.Omschrijving = Convert.ToString(rdr["Omschrijving"]);
                    spel.AandeBeurt = (Kleur)Convert.ToInt32(rdr["AandeBeurt"]);
                    spel.Bord[Convert.ToInt32(rdr["Row"]), Convert.ToInt32(rdr["Col"])] = (Kleur)Convert.ToInt32(rdr["Kleur"]);
                    spel.Afgelopen = Convert.ToBoolean(rdr["Afgelopen"]);
                    spel.Winnaar = Convert.ToString(rdr["Winnaar"]);

                }
                sqlCon.Close();               
            }
            return spel;
        }

        public List<Spel> GetSpellen()
        {
            var spelList = new List<Spel>();
            string sqlQuery = "SELECT Token FROM Games";

            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlCon);
                SqlDataReader rdr = sqlCmd.ExecuteReader();

                while (rdr.Read())
                {
                    spelList.Add(GetSpel(Convert.ToString(rdr["Token"])));
                }
            }
            return spelList;
        }

        public void DeleteSpel(string spelToken)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                string verwijderSpelQuery = "DELETE FROM Games WHERE Token = @spelToken";
                string verwijderCellQuery = "DELETE FROM Cell WHERE Token = @spelToken";

                SqlCommand sqlCmdSpel = new SqlCommand(verwijderSpelQuery, sqlCon);
                SqlCommand sqlCmdCell = new SqlCommand(verwijderCellQuery, sqlCon);

                sqlCmdSpel.Parameters.AddWithValue("@spelToken", spelToken);
                sqlCmdCell.Parameters.AddWithValue("@spelToken", spelToken);

                sqlCon.Open();
                sqlCmdSpel.ExecuteNonQuery();
                sqlCmdCell.ExecuteNonQuery();
                sqlCon.Close();
            }
        }

        public void UpdateSpel(Spel spel)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                //INSET querys for game and bord
                string UpdateSpelQuery = "UPDATE Games SET AandeBeurt = @AandeBeurt, Afgelopen = @Afgelopen, Winnaar = @Winnaar WHERE Token = @Token";
                string UpdateBordQuery = "UPDATE Cell SET  Kleur = @Kleur WHERE Token = @Token AND Row = @Row AND Col = @Col";

                SqlCommand sqlCmd = new SqlCommand(UpdateSpelQuery, sqlCon);

                sqlCmd.Parameters.AddWithValue("@Token", spel.Token);
                sqlCmd.Parameters.AddWithValue("@AandeBeurt", spel.AandeBeurt);
                sqlCmd.Parameters.AddWithValue("@Afgelopen", spel.Afgelopen);
                sqlCmd.Parameters.AddWithValue("@Winnaar", spel.Winnaar);

                sqlCon.Open();
                sqlCmd.ExecuteNonQuery();

                //Loops over game bord and adds all cells with x and y locations and the colour that occupies the space and the game token
                for (int omlaag = 0; omlaag < spel.Bord.GetLength(0); omlaag++)
                {
                    for (int opzij = 0; opzij < spel.Bord.GetLength(1); opzij++)
                    {
                        SqlCommand sqlCmdAddBord = new SqlCommand(UpdateBordQuery, sqlCon);
                        sqlCmdAddBord.Parameters.AddWithValue("@Token", spel.Token);
                        sqlCmdAddBord.Parameters.AddWithValue("@Row", opzij);
                        sqlCmdAddBord.Parameters.AddWithValue("@Col", omlaag);
                        sqlCmdAddBord.Parameters.AddWithValue("@Kleur", spel.Bord[opzij, omlaag]);
                        sqlCmdAddBord.ExecuteNonQuery();
                    }
                }

                sqlCon.Close();

            }
        }
        public List<int> GetPieceHistory(string spelToken, string spelerToken)
        {
            var pieceList = new List<int>();
            string sqlQuery = "SELECT Amount FROM PieceHistory WHERE GameToken = @GameToken AND PlayerToken = @PlayerToken";

            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlCon);
                sqlCmd.Parameters.AddWithValue("@GameToken", spelToken);
                sqlCmd.Parameters.AddWithValue("@PlayerToken", spelerToken);
                SqlDataReader rdr = sqlCmd.ExecuteReader();

                while (rdr.Read())
                {
                    pieceList.Add(Convert.ToInt32(rdr["Amount"]));
                }
            }
            return pieceList;
        }

        public void AddPieceHistoryteSpel(string spelToken, int aantal1, int aantal2)
        {
            var spel = GetSpel(spelToken);
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                string speler1SpelQuery = "INSERT INTO PieceHistory (GameToken, PlayerToken, Amount) VALUES(@GameToken, @PlayerToken, @Amount)";
                string speler2SpelQuery = "INSERT INTO PieceHistory (GameToken, PlayerToken, Amount) VALUES(@GameToken, @PlayerToken, @Amount)";

                SqlCommand sqlCmd1 = new SqlCommand(speler1SpelQuery, sqlCon);
                SqlCommand sqlCmd2 = new SqlCommand(speler2SpelQuery, sqlCon);

                sqlCmd1.Parameters.AddWithValue("@GameToken", spelToken);
                sqlCmd1.Parameters.AddWithValue("@PlayerToken", spel.Speler1Token);
                sqlCmd1.Parameters.AddWithValue("@Amount", aantal1);
                sqlCmd2.Parameters.AddWithValue("@GameToken", spelToken);
                sqlCmd2.Parameters.AddWithValue("@PlayerToken", spel.Speler2Token);
                sqlCmd2.Parameters.AddWithValue("@Amount", aantal2);

                sqlCon.Open();
                sqlCmd1.ExecuteNonQuery();
                sqlCmd2.ExecuteNonQuery();
                sqlCon.Close();

            }
        }

        public void OpgevenSpel(Spel spel)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                string OpgevenSpelQuery = "UPDATE Games SET Winnaar = @Winnaar, Afgelopen = @Afgelopen WHERE Token = @Token";

                SqlCommand sqlCmd = new SqlCommand(OpgevenSpelQuery, sqlCon);

                sqlCmd.Parameters.AddWithValue("@Token", spel.Token);
                sqlCmd.Parameters.AddWithValue("@Winnaar", spel.Winnaar);
                sqlCmd.Parameters.AddWithValue("@Afgelopen", spel.Afgelopen);

                sqlCon.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();

            }
        }
    }
}
