using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using PraktikaNORBIT_UniversitySQL_DataBase.Models;

namespace PraktikaNORBIT_UniversitySQL_DataBase.Data.Repositories
{
    public class ScheduleRepository
    {
        private readonly string _connectionString;

        public ScheduleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Schedule schedule)
        {
            string sql = @"
                INSERT INTO Schedule 
                (Id, GroupId, SubjectId, TeacherId, DateTime, Classroom) 
                VALUES 
                (@Id, @GroupId, @SubjectId, @TeacherId, @DateTime, @Classroom)";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", schedule.Id);
                command.Parameters.AddWithValue("@GroupId", schedule.GroupId);
                command.Parameters.AddWithValue("@SubjectId", schedule.SubjectId);
                command.Parameters.AddWithValue("@TeacherId", schedule.TeacherId);
                command.Parameters.AddWithValue("@DateTime", schedule.DateTime);
                command.Parameters.AddWithValue("@Classroom", schedule.Classroom);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Schedule> GetAll()
        {
            var schedules = new List<Schedule>();
            string sql = "SELECT Id, GroupId, SubjectId, TeacherId, DateTime, Classroom FROM Schedule";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schedules.Add(new Schedule
                        {
                            Id = reader.GetGuid(0),
                            GroupId = reader.GetGuid(1),
                            SubjectId = reader.GetGuid(2),
                            TeacherId = reader.GetGuid(3),
                            DateTime = reader.GetDateTime(4),
                            Classroom = reader.GetString(5)
                        });
                    }
                }
            }

            return schedules;
        }

        public List<Schedule> GetByGroup(Guid groupId)
        {
            var schedules = new List<Schedule>();
            string sql = "SELECT Id, GroupId, SubjectId, TeacherId, DateTime, Classroom FROM Schedule WHERE GroupId = @GroupId";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@GroupId", groupId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schedules.Add(new Schedule
                        {
                            Id = reader.GetGuid(0),
                            GroupId = reader.GetGuid(1),
                            SubjectId = reader.GetGuid(2),
                            TeacherId = reader.GetGuid(3),
                            DateTime = reader.GetDateTime(4),
                            Classroom = reader.GetString(5)
                        });
                    }
                }
            }

            return schedules;
        }

        public void Update(Schedule schedule)
        {
            string sql = @"
                UPDATE Schedule 
                SET GroupId = @GroupId, 
                    SubjectId = @SubjectId, 
                    TeacherId = @TeacherId, 
                    DateTime = @DateTime, 
                    Classroom = @Classroom 
                WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", schedule.Id);
                command.Parameters.AddWithValue("@GroupId", schedule.GroupId);
                command.Parameters.AddWithValue("@SubjectId", schedule.SubjectId);
                command.Parameters.AddWithValue("@TeacherId", schedule.TeacherId);
                command.Parameters.AddWithValue("@DateTime", schedule.DateTime);
                command.Parameters.AddWithValue("@Classroom", schedule.Classroom);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Guid id)
        {
            string sql = "DELETE FROM Schedule WHERE Id = @Id";

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
