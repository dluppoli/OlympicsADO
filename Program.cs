using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Configuration;
using System.Configuration;

namespace OlympicsADO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Stabilire una connessione
            string connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;
            Console.WriteLine("1 - Stampa numero righe");
            Console.WriteLine("2 - Ricerca per IdAthlete");
            Console.WriteLine("3 - Aggiorna Olimpiadi Tokyo 2020");
            Console.WriteLine("4 - Nuova partecipazione");

            string scelta = Console.ReadLine();

            if(scelta == "1")
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    try
                    {
                        sqlConnection.Open();

                        //Creazione del comando
                        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM AthletesFull",sqlConnection);
                        //cmd.CommandText = "SELECT COUNT(*) FROM AthletesFull";
                        //cmd.Connection = sqlConnection;

                        //Eseguire il comando e gestire il valore restituito dal database
                        int risultato = (int)cmd.ExecuteScalar();
                        Console.WriteLine(risultato);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            if (scelta == "2")
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    try
                    {
                        List<Athlete> risultati = new List<Athlete>();

                        sqlConnection.Open();
                        Console.Write("Inserire id athleta: ");
                        if (int.TryParse(Console.ReadLine(), out int idAthlete))
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandText = "SELECT * FROM AthletesFull where IdAthlete = @idAthlete order by Id";
                            cmd.Connection = sqlConnection;
                            cmd.Parameters.AddWithValue("@idAthlete", idAthlete);

                            using ( SqlDataReader reader = cmd.ExecuteReader() )
                            {
                                while (reader.Read()) {

                                    Athlete nuovo = new Athlete()
                                    {
                                        Id = (long)reader["Id"],
                                        IdAthlete = reader.IsDBNull(1) ? null : (long?)reader["IdAthlete"],
                                        Name = reader.IsDBNull(2) ? "" : (string)reader["Name"],
                                        Sex = reader.IsDBNull(3) ? null : (char?)Convert.ToChar(reader["Sex"]),
                                        Age = reader.IsDBNull(4) ? null : (int?)reader["Age"],
                                        Height = reader.IsDBNull(5) ? null : (int?)reader["Height"],
                                        Weight = reader.IsDBNull(6) ? null : (int?)reader["Weight"],
                                        NOC = reader.IsDBNull(7) ? null : reader["NOC"].ToString(),
                                        Games = reader.IsDBNull(8) ? null : reader["Games"].ToString(),
                                        Year = reader.IsDBNull(9) ? null : (int?)reader["Year"],
                                        Season = reader.IsDBNull(10) ? null : reader["Season"].ToString(),
                                        City = reader.IsDBNull(11) ? null : reader["City"].ToString(),
                                        Sport = reader.IsDBNull(12) ? null : reader["Sport"].ToString(),
                                        Event = reader.IsDBNull(13) ? null : reader["Event"].ToString(),
                                        Medal = reader["Medal"]?.ToString()
                                    };
                                    risultati.Add(nuovo);
                                }
                            }
                            if(risultati.Count > 0)
                                risultati.ForEach(a => Console.WriteLine($"{a.Name} - {a.Year} - {a.Event} - {a.Medal}"));
                            else
                                Console.WriteLine("Atleta non trovato");

                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            if (scelta == "3")
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = $"UPDATE AthletesFull SET Year=2021 WHERE Year=2020";
                        cmd.Connection = sqlConnection;

                        Console.WriteLine($"Aggiornate {cmd.ExecuteNonQuery()} righe");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }

            if (scelta == "4")
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    try
                    {
                        sqlConnection.Open();

                        Console.Write("Inserire il nome: ");

                        Athlete nuovo = new Athlete();
                        nuovo.Name = Console.ReadLine();

                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection= sqlConnection;
                        cmd.CommandText = "INSERT INTO AthletesFull(Name) VALUES(@name)";

                        //SqlParameter param = new SqlParameter("@name",nuovo.Name);
                        //cmd.Parameters.Add(param);

                        cmd.Parameters.AddWithValue("@name", nuovo.Name);

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
