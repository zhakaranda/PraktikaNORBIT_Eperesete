using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using PraktikaNORBIT_UniversitySQL_DataBase.Models;

namespace PraktikaNORBIT_UniversitySQL_DataBase.Data.Repositories
{
    public class TeacherRepository
    {
        private readonly string _connectionString;

        public TeacherRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Teacher teacher)
        {
            string sql = @"
                INSERT INTO Teachers 
                (Id, FullName, BirthDate, Department, AcademicDegree, HireDate, Salary) 
                VALUES 
                (@Id, @FullName, @BirthDate, @Department, @AcademicDegree, @HireDate, @Salary)";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", teacher.Id);
                command.Parameters.AddWithValue("@FullName", teacher.FullName);
                command.Parameters.AddWithValue("@BirthDate", (object)teacher.BirthDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@Department", teacher.Department);
                command.Parameters.AddWithValue("@AcademicDegree", (object)teacher.AcademicDegree ?? DBNull.Value);
                command.Parameters.AddWithValue("@HireDate", teacher.HireDate);
                command.Parameters.AddWithValue("@Salary", teacher.Salary);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Teacher> GetAll()
        {
            var teachers = new List<Teacher>();
            string sql = "SELECT Id, FullName, BirthDate, Department, AcademicDegree, HireDate, Salary FROM Teachers";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        teachers.Add(new Teacher
                        {
                            Id = reader.GetGuid(0),
                            FullName = reader.GetString(1),
                            BirthDate = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                            Department = reader.GetString(3),
                            AcademicDegree = reader.IsDBNull(4) ? null : reader.GetString(4),
                            HireDate = reader.GetDateTime(5),
                            Salary = reader.GetDecimal(6)
                        });
                    }
                }
            }

            return teachers;
        }

        public Teacher GetById(Guid id)
        {
            string sql = "SELECT Id, FullName, BirthDate, Department, AcademicDegree, HireDate, Salary FROM Teachers WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Teacher
                        {
                            Id = reader.GetGuid(0),
                            FullName = reader.GetString(1),
                            BirthDate = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                            Department = reader.GetString(3),
                            AcademicDegree = reader.IsDBNull(4) ? null : reader.GetString(4),
                            HireDate = reader.GetDateTime(5),
                            Salary = reader.GetDecimal(6)
                        };
                    }
                }
            }

            return null;
        }

        public void Update(Teacher teacher)
        {
            string sql = @"
                UPDATE Teachers 
                SET FullName = @FullName, 
                    BirthDate = @BirthDate, 
                    Department = @Department, 
                    AcademicDegree = @AcademicDegree, 
                    HireDate = @HireDate, 
                    Salary = @Salary 
                WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", teacher.Id);
                command.Parameters.AddWithValue("@FullName", teacher.FullName);
                command.Parameters.AddWithValue("@BirthDate", (object)teacher.BirthDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@Department", teacher.Department);
                command.Parameters.AddWithValue("@AcademicDegree", (object)teacher.AcademicDegree ?? DBNull.Value);
                command.Parameters.AddWithValue("@HireDate", teacher.HireDate);
                command.Parameters.AddWithValue("@Salary", teacher.Salary);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Guid id)
        {
            string sql = "DELETE FROM Teachers WHERE Id = @Id";

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
