using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using PraktikaNORBIT_UniversitySQL_DataBase.Models;

namespace PraktikaNORBIT_UniversitySQL_DataBase.Data.Repositories
{
    public class SubjectRepository
    {
        private readonly string _connectionString;

        public SubjectRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Subject subject)
        {
            string sql = @"
                INSERT INTO Subjects 
                (Id, Name, Credits, Hours, IsExam, Department) 
                VALUES 
                (@Id, @Name, @Credits, @Hours, @IsExam, @Department)";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", subject.Id);
                command.Parameters.AddWithValue("@Name", subject.Name);
                command.Parameters.AddWithValue("@Credits", subject.Credits);
                command.Parameters.AddWithValue("@Hours", subject.Hours);
                command.Parameters.AddWithValue("@IsExam", subject.IsExam);
                command.Parameters.AddWithValue("@Department", subject.Department);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Subject> GetAll()
        {
            var subjects = new List<Subject>();
            string sql = "SELECT Id, Name, Credits, Hours, IsExam, Department FROM Subjects";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subjects.Add(new Subject
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Credits = reader.GetDecimal(2),
                            Hours = reader.GetInt32(3),
                            IsExam = reader.GetBoolean(4),
                            Department = reader.GetString(5)
                        });
                    }
                }
            }

            return subjects;
        }

        public Subject GetById(Guid id)
        {
            string sql = "SELECT Id, Name, Credits, Hours, IsExam, Department FROM Subjects WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Subject
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Credits = reader.GetDecimal(2),
                            Hours = reader.GetInt32(3),
                            IsExam = reader.GetBoolean(4),
                            Department = reader.GetString(5)
                        };
                    }
                }
            }

            return null;
        }

        public List<Subject> GetByDepartment(string department)
        {
            var subjects = new List<Subject>();
            string sql = "SELECT Id, Name, Credits, Hours, IsExam, Department FROM Subjects WHERE Department = @Department";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Department", department);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subjects.Add(new Subject
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Credits = reader.GetDecimal(2),
                            Hours = reader.GetInt32(3),
                            IsExam = reader.GetBoolean(4),
                            Department = reader.GetString(5)
                        });
                    }
                }
            }

            return subjects;
        }

        public void Update(Subject subject)
        {
            string sql = @"
                UPDATE Subjects 
                SET Name = @Name, 
                    Credits = @Credits, 
                    Hours = @Hours, 
                    IsExam = @IsExam, 
                    Department = @Department 
                WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", subject.Id);
                command.Parameters.AddWithValue("@Name", subject.Name);
                command.Parameters.AddWithValue("@Credits", subject.Credits);
                command.Parameters.AddWithValue("@Hours", subject.Hours);
                command.Parameters.AddWithValue("@IsExam", subject.IsExam);
                command.Parameters.AddWithValue("@Department", subject.Department);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Guid id)
        {
            string sql = "DELETE FROM Subjects WHERE Id = @Id";

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
