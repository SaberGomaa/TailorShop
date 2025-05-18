using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace TailorShop
{
    public class DatabaseHelper
    {
        private readonly string dbPath = "TailorShop.db";
        private readonly string connectionString;

        public DatabaseHelper()
        {
            connectionString = $"Data Source={dbPath};Version=3;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(
                    @"CREATE TABLE IF NOT EXISTS Customers (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Length REAL,
                        Sleeve REAL,
                        Shoulder REAL,
                        Width REAL,
                        Fitness REAL,
                        Collar REAL,
                        Embroidery TEXT,
                        Phone TEXT,
                        Notes TEXT
                    )", connection);
                command.ExecuteNonQuery();
            }
        }

        public void InsertCustomer(Customer customer)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(
                    @"INSERT INTO Customers (Name, Length, Sleeve, Shoulder, Width, Fitness, Collar, Embroidery, Phone, Notes)
                      VALUES (@Name, @Length, @Sleeve, @Shoulder, @Width, @Fitness, @Collar, @Embroidery, @Phone, @Notes);
                      SELECT last_insert_rowid();", connection);
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@Length", customer.Length);
                command.Parameters.AddWithValue("@Sleeve", customer.Sleeve);
                command.Parameters.AddWithValue("@Shoulder", customer.Shoulder);
                command.Parameters.AddWithValue("@Width", customer.Width);
                command.Parameters.AddWithValue("@Fitness", customer.Fitness);
                command.Parameters.AddWithValue("@Collar", customer.Collar);
                command.Parameters.AddWithValue("@Embroidery", (object)customer.Embroidery ?? DBNull.Value);
                command.Parameters.AddWithValue("@Phone", (object)customer.Phone ?? DBNull.Value);
                command.Parameters.AddWithValue("@Notes", (object)customer.Notes ?? DBNull.Value);

                customer.ID = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(
                    @"UPDATE Customers SET Name = @Name, Length = @Length, Sleeve = @Sleeve, Shoulder = @Shoulder,
                      Width = @Width, Fitness = @Fitness, Collar = @Collar, Embroidery = @Embroidery, Phone = @Phone,
                      Notes = @Notes WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", customer.ID);
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@Length", customer.Length);
                command.Parameters.AddWithValue("@Sleeve", customer.Sleeve);
                command.Parameters.AddWithValue("@Shoulder", customer.Shoulder);
                command.Parameters.AddWithValue("@Width", customer.Width);
                command.Parameters.AddWithValue("@Fitness", customer.Fitness);
                command.Parameters.AddWithValue("@Collar", customer.Collar);
                command.Parameters.AddWithValue("@Embroidery", (object)customer.Embroidery ?? DBNull.Value);
                command.Parameters.AddWithValue("@Phone", (object)customer.Phone ?? DBNull.Value);
                command.Parameters.AddWithValue("@Notes", (object)customer.Notes ?? DBNull.Value);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteCustomer(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("DELETE FROM Customers WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();
            }
        }

        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Customers", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Length = reader.GetDouble(2),
                            Sleeve = reader.GetDouble(3),
                            Shoulder = reader.GetDouble(4),
                            Width = reader.GetDouble(5),
                            Fitness = reader.GetDouble(6),
                            Collar = reader.GetDouble(7),
                            Embroidery = reader.IsDBNull(8) ? null : reader.GetString(8),
                            Phone = reader.IsDBNull(9) ? null : reader.GetString(9),
                            Notes = reader.IsDBNull(10) ? null : reader.GetString(10)
                        });
                    }
                }
            }
            return customers;
        }

        public List<Customer> SearchCustomers(string searchText)
        {
            var customers = new List<Customer>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Customers WHERE Name LIKE @SearchText", connection);
                command.Parameters.AddWithValue("@SearchText", $"%{searchText}%");
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Length = reader.GetDouble(2),
                            Sleeve = reader.GetDouble(3),
                            Shoulder = reader.GetDouble(4),
                            Width = reader.GetDouble(5),
                            Fitness = reader.GetDouble(6),
                            Collar = reader.GetDouble(7),
                            Embroidery = reader.IsDBNull(8) ? null : reader.GetString(8),
                            Phone = reader.IsDBNull(9) ? null : reader.GetString(9),
                            Notes = reader.IsDBNull(10) ? null : reader.GetString(10)
                        });
                    }
                }
            }
            return customers;
        }
    }
}