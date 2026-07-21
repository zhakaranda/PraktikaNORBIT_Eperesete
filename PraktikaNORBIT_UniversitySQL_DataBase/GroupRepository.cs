using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using PraktikaNORBIT_UniversitySQL_DataBase.Models;

namespace PraktikaNORBIT_UniversitySQL_DataBase.Data.Repositories
{
    public class GroupRepository
    {
        private readonly string _connectionString;

        public GroupRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE
        public void Add(Group group)
        {
            string sql = @"
                INSERT INTO Groups 
                (Id, Name, FacultyId, Course, CuratorId, Year) 
                VALUES 
                (@Id, @Name, @FacultyId, @Course, @CuratorId, @Year)";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", group.Id);
                command.Parameters.AddWithValue("@Name", group.Name);
                command.Parameters.AddWithValue("@FacultyId", group.FacultyId);
                command.Parameters.AddWithValue("@Course", group.Course);
                command.Parameters.AddWithValue("@CuratorId", (object)group.CuratorId ?? DBNull.Value);
                command.Parameters.AddWithValue("@Year", group.Year);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // READ - все группы
        public List<Group> GetAll()
        {
            var groups = new List<Group>();
            string sql = "SELECT Id, Name, FacultyId, Course, CuratorId, Year FROM Groups";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groups.Add(new Group
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            FacultyId = reader.GetGuid(2),
                            Course = reader.GetInt32(3),
                            CuratorId = reader.IsDBNull(4) ? (Guid?)null : reader.GetGuid(4),
                            Year = reader.GetInt32(5)
                        });
                    }
                }
            }

            return groups;
        }

        // READ - группа по ID
        public Group GetById(Guid id)
        {
            string sql = "SELECT Id, Name, FacultyId, Course, CuratorId, Year FROM Groups WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Group
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            FacultyId = reader.GetGuid(2),
                            Course = reader.GetInt32(3),
                            CuratorId = reader.IsDBNull(4) ? (Guid?)null : reader.GetGuid(4),
                            Year = reader.GetInt32(5)
                        };
                    }
                }
            }

            return null;
        }

        // READ - группы по факультету
        public List<Group> GetByFaculty(Guid facultyId)
        {
            var groups = new List<Group>();
            string sql = "SELECT Id, Name, FacultyId, Course, CuratorId, Year FROM Groups WHERE FacultyId = @FacultyId";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@FacultyId", facultyId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groups.Add(new Group
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            FacultyId = reader.GetGuid(2),
                            Course = reader.GetInt32(3),
                            CuratorId = reader.IsDBNull(4) ? (Guid?)null : reader.GetGuid(4),
                            Year = reader.GetInt32(5)
                        });
                    }
                }
            }

            return groups;
        }

        // UPDATE
        public void Update(Group group)
        {
            string sql = @"
                UPDATE Groups 
                SET Name = @Name, 
                    FacultyId = @FacultyId, 
                    Course = @Course, 
                    CuratorId = @CuratorId, 
                    Year = @Year 
                WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", group.Id);
                command.Parameters.AddWithValue("@Name", group.Name);
                command.Parameters.AddWithValue("@FacultyId", group.FacultyId);
                command.Parameters.AddWithValue("@Course", group.Course);
                command.Parameters.AddWithValue("@CuratorId", (object)group.CuratorId ?? DBNull.Value);
                command.Parameters.AddWithValue("@Year", group.Year);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // DELETE
        public void Delete(Guid id)
        {
            string sql = "DELETE FROM Groups WHERE Id = @Id";

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
