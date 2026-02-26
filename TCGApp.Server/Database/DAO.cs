namespace TCGApp.Server.Database;
using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TCGApp.Server.Models;

    public class DAO
    {

        public readonly string _connectionString;
        public DAO(IConfiguration configuration)
        {
           _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public List<Card> GetCards()
        {
            var cards = new List<Card>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Card", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var card = new Card
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CardName = reader.GetString(reader.GetOrdinal("CardName")),
                            CardGame = reader.GetString(reader.GetOrdinal("CardGame")),
                            CardType = reader.GetString(reader.GetOrdinal("CardType")),
                            Rarity = reader.GetString(reader.GetOrdinal("Rarity")),
                            Set = reader.GetString(reader.GetOrdinal("Set")),
                            Price = reader.GetFloat(reader.GetOrdinal("Price")),
                            // Assuming SpecialProperties and InGameProperties are stored as comma-separated strings
                            SpecialProperties = reader.GetString(reader.GetOrdinal("SpecialProperties")).Split(',').ToList(),
                            InGameProperties = reader.GetString(reader.GetOrdinal("InGameProperties")).Split(',').ToList()
                        };
                        cards.Add(card);
                    }
                }
            }
            return cards;
        }

        public void CreateUser(TCGUser newUser)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO TCGUser (Username, Email, PasswordHash, LastLogin) VALUES (@Username, @Email, @PasswordHash, @LastLogin)", connection);
                command.Parameters.AddWithValue("@Username", newUser.Username);
                command.Parameters.AddWithValue("@Email", newUser.Email);
                command.Parameters.AddWithValue("@PasswordHash", newUser.PasswordHash);
                command.Parameters.AddWithValue("@LastLogin", newUser.LastLogin);
                command.Prepare();
                command.ExecuteNonQuery();
            }
        }
    }
