using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using PraktikaNORBIT_UniversitySQL_DataBase.Models;

namespace PraktikaNORBIT_UniversitySQL_DataBase.Data.Repositories
{
    public class FacultyRepository
    {
        private readonly string _connectionString;

        public FacultyRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Faculty faculty)
        {
            string sql = @"
                INSERT INTO Faculties 
                (Id, Name, DeanId, Phone, Email, FoundationDate) 
                VALUES 
                (@Id, @Name, @DeanId, @Phone, @Email, @FoundationDate)";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", faculty.Id);
                command.Parameters.AddWithValue("@Name", faculty.Name);
                command.Parameters.AddWithValue("@DeanId", faculty.DeanId);
                command.Parameters.AddWithValue("@Phone", (object)faculty.Phone ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", (object)faculty.Email ?? DBNull.Value);
                command.Parameters.AddWithValue("@FoundationDate", faculty.FoundationDate);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Faculty> GetAll()
        {
            var faculties = new List<Faculty>();
            string sql = "SELECT Id, Name, DeanId, Phone, Email, FoundationDate FROM Faculties";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        faculties.Add(new Faculty
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            DeanId = reader.GetGuid(2),
                            Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                            FoundationDate = reader.GetDateTime(5)
                        });
                    }
                }
            }

            return faculties;
        }

        public Faculty GetById(Guid id)
        {
            string sql = "SELECT Id, Name, DeanId, Phone, Email, FoundationDate FROM Faculties WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Faculty
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            DeanId = reader.GetGuid(2),
                            Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                            FoundationDate = reader.GetDateTime(5)
                        };
                    }
                }
            }

            return null;
        }

        public void Update(Faculty faculty)
        {
            string sql = @"
                UPDATE Faculties 
                SET Name = @Name, 
                    DeanId = @DeanId, 
                    Phone = @Phone, 
                    Email = @Email, 
                    FoundationDate = @FoundationDate 
                WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", faculty.Id);
                command.Parameters.AddWithValue("@Name", faculty.Name);
                command.Parameters.AddWithValue("@DeanId", faculty.DeanId);
                command.Parameters.AddWithValue("@Phone", (object)faculty.Phone ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", (object)faculty.Email ?? DBNull.Value);
                command.Parameters.AddWithValue("@FoundationDate", faculty.FoundationDate);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Guid id)
        {
            string sql = "DELETE FROM Faculties WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
