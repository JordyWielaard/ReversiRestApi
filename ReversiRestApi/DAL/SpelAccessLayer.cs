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

        //SQL querys to allow insertation into db
        private const string IdOn = "SET IDENTITY_INSERT Games ON";
        private const string IdOff = "SET IDENTITY_INSERT Games Off";

        //Insert game to database table & insert all cell values into cell table with the game token
        public void AddSpel(Spel spel)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                //INSET querys for game and bord
                string addSpelQuery = "INSERT INTO Games (ID, Token, Speler1Token, Speler2Token, Omschrijving, AandeBeurt) VALUES(@ID, @Token, @Speler1Token, @Speler2Token, @Omschrijving, @AandeBeurt)";
                string addBordQuery = "INSERT INTO Cell (Token, Row, Col, Kleur) VALUES (@Token, @Row, @Col, @Kleur)";
               
                SqlCommand sqlCmdOn = new SqlCommand(IdOn, sqlCon);
                SqlCommand sqlCmdOff = new SqlCommand(IdOff, sqlCon);
                 
                SqlCommand sqlCmd = new SqlCommand(addSpelQuery, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ID", spel.ID);
                sqlCmd.Parameters.AddWithValue("@Token", spel.Token);
                sqlCmd.Parameters.AddWithValue("@Speler1Token", spel.Speler1Token);
                sqlCmd.Parameters.AddWithValue("@Speler2Token", spel.Speler2Token);
                sqlCmd.Parameters.AddWithValue("@Omschrijving", spel.Omschrijving);
                sqlCmd.Parameters.AddWithValue("@AandeBeurt", spel.AandeBeurt);

                sqlCon.Open();
                sqlCmdOn.ExecuteNonQuery();
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
                
                sqlCmdOff.ExecuteNonQuery();
                sqlCon.Close();

            }
        }

        //TODO: Add query to get bord from database
        public Spel GetSpel(string spelToken)
        {
            var spel = new Spel();
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Games JOIN Cell ON Games.Token = Cell.Token WHERE Games.Token = '" + spelToken + "'";
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                SqlDataReader rdr = sqlCmd.ExecuteReader();

                while (rdr.Read())
                {
                    spel.ID = Convert.ToInt32(rdr["ID"]);
                    spel.Token = Convert.ToString(rdr["Token"]);
                    spel.Speler1Token = Convert.ToString(rdr["Speler1Token"]);
                    spel.Speler2Token = Convert.ToString(rdr["Speler2Token"]);
                    spel.Omschrijving = Convert.ToString(rdr["Omschrijving"]);
                    spel.AandeBeurt = (Kleur)Convert.ToInt32(rdr["AandeBeurt"]);
                    spel.Bord[Convert.ToInt32(rdr["Col"]), Convert.ToInt32(rdr["Row"])] = (Kleur)Convert.ToInt32(rdr["Kleur"]);
                }
                sqlCon.Close();               
            }
            return spel;
        }

        public List<Spel> GetSpellen()
        {
            var spelList = new List<Spel>();
            string sqlQuery = "SELECT * FROM Games JOIN Cell ON Games.Token = Cell.Token";

            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlCon);
                SqlDataReader rdr = sqlCmd.ExecuteReader();

                while (rdr.Read())
                {
                    var spel = new Spel();
                    spel.ID = Convert.ToInt32(rdr["ID"]);
                    spel.Token = Convert.ToString(rdr["Token"]);
                    spel.Speler1Token = Convert.ToString(rdr["Speler1Token"]);
                    spel.Speler2Token = Convert.ToString(rdr["Speler2Token"]);
                    spel.Omschrijving = Convert.ToString(rdr["Omschrijving"]);
                    spel.AandeBeurt = (Kleur)Convert.ToInt32(rdr["AandeBeurt"]);
                    spel.Bord[Convert.ToInt32(rdr["Col"]), Convert.ToInt32(rdr["Row"])] = (Kleur)Convert.ToInt32(rdr["Kleur"]);
                    spelList.Add(spel);
                }
            }
            return spelList;
        }
    }
}
