using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using PraktikaNORBIT_UniversitySQL_DataBase.Models;

namespace PraktikaNORBIT_UniversitySQL_DataBase.Data.Repositories
{
    public class GradeRepository
    {
        private readonly string _connectionString;

        public GradeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Grade grade)
        {
            string sql = @"
                INSERT INTO Grades 
                (Id, StudentId, SubjectId, Grade, Date, IsRetake) 
                VALUES 
                (@Id, @StudentId, @SubjectId, @Grade, @Date, @IsRetake)";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", grade.Id);
                command.Parameters.AddWithValue("@StudentId", grade.StudentId);
                command.Parameters.AddWithValue("@SubjectId", grade.SubjectId);
                command.Parameters.AddWithValue("@Grade", grade.GradeValue);
                command.Parameters.AddWithValue("@Date", grade.Date);
                command.Parameters.AddWithValue("@IsRetake", grade.IsRetake);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Grade> GetAll()
        {
            var grades = new List<Grade>();
            string sql = "SELECT Id, StudentId, SubjectId, Grade, Date, IsRetake FROM Grades";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        grades.Add(new Grade
                        {
                            Id = reader.GetGuid(0),
                            StudentId = reader.GetGuid(1),
                            SubjectId = reader.GetGuid(2),
                            GradeValue = reader.GetInt32(3),
                            Date = reader.GetDateTime(4),
                            IsRetake = reader.GetBoolean(5)
                        });
                    }
                }
            }

            return grades;
        }

        public List<Grade> GetByStudent(Guid studentId)
        {
            var grades = new List<Grade>();
            string sql = "SELECT Id, StudentId, SubjectId, Grade, Date, IsRetake FROM Grades WHERE StudentId = @StudentId";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@StudentId", studentId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        grades.Add(new Grade
                        {
                            Id = reader.GetGuid(0),
                            StudentId = reader.GetGuid(1),
                            SubjectId = reader.GetGuid(2),
                            GradeValue = reader.GetInt32(3),
                            Date = reader.GetDateTime(4),
                            IsRetake = reader.GetBoolean(5)
                        });
                    }
                }
            }

            return grades;
        }

        public void Update(Grade grade)
        {
            string sql = @"
                UPDATE Grades 
                SET Grade = @Grade, 
                    Date = @Date, 
                    IsRetake = @IsRetake 
                WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", grade.Id);
                command.Parameters.AddWithValue("@Grade", grade.GradeValue);
                command.Parameters.AddWithValue("@Date", grade.Date);
                command.Parameters.AddWithValue("@IsRetake", grade.IsRetake);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Guid id)
        {
            string sql = "DELETE FROM Grades WHERE Id = @Id";

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
