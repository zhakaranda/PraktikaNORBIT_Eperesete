using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using PraktikaNORBIT_UniversitySQL_DataBase.Models;

namespace PraktikaNORBIT_UniversitySQL_DataBase.Data.Repositories
{
    //Репозиторий для работы с таблицей Students через ADO.NET
    public class StudentRepository
    {
        // Храним строку подключения в поле класса
        // readonly - поле можно инициализировать только в конструкторе
        private readonly string _connectionString;

        // Конструктор принимает строку подключения
        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 1. CREATE - Добавление студента
        //Добавляет нового студента в БД
        public void Add(Student student)
        {
            // SQL-запрос на вставку данных
            // @Id, @FullName и т.д. - это параметры (защита от SQL-инъекций)
            string sql = @"
                INSERT INTO Students 
                (Id, FullName, BirthDate, GroupId, Email, IsBudget, EnrollmentDate) 
                VALUES 
                (@Id, @FullName, @BirthDate, @GroupId, @Email, @IsBudget, @EnrollmentDate)";

            // using гарантирует, что ресурсы будут освобождены после использования
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                // Добавляем параметры в запрос
                // Это БЕЗОПАСНО! Параметры защищают от SQL-инъекций
                command.Parameters.AddWithValue("@Id", student.Id);
                command.Parameters.AddWithValue("@FullName", student.FullName);
                command.Parameters.AddWithValue("@BirthDate", student.BirthDate);
                command.Parameters.AddWithValue("@GroupId", student.GroupId);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@IsBudget", student.IsBudget);
                command.Parameters.AddWithValue("@EnrollmentDate", student.EnrollmentDate);

                // Открываем соединение
                connection.Open();

                // Выполняем запрос (не возвращает данные)
                command.ExecuteNonQuery();

                // Соединение закроется автоматически при выходе из using
            }
        }

        // 2. READ - Чтение данных
        // Получает всех студентов из БД
        public List<Student> GetAll()
        {
            // Создаём пустой список для результатов
            var students = new List<Student>();

            // SQL-запрос на выборку всех студентов
            string sql = "SELECT Id, FullName, BirthDate, GroupId, Email, IsBudget, EnrollmentDate FROM Students";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();

                // ExecuteReader - читаем данные построчно
                using (var reader = command.ExecuteReader())
                {
                    // Read() возвращает true, пока есть строки
                    while (reader.Read())
                    {
                        // Создаём объект Student и заполняем его из reader
                        students.Add(new Student
                        {
                            // reader.GetGuid(0) - читаем колонку с индексом 0 как Guid
                            Id = reader.GetGuid(0),

                            // reader.GetString(1) - читаем как строку
                            FullName = reader.GetString(1),

                            // reader.GetDateTime(2) - читаем как дату
                            BirthDate = reader.GetDateTime(2),

                            GroupId = reader.GetGuid(3),
                            Email = reader.GetString(4),

                            // reader.GetBoolean(5) - читаем как bool
                            IsBudget = reader.GetBoolean(5),

                            EnrollmentDate = reader.GetDateTime(6)
                        });
                    }
                }
            }

            return students;
        }

        // Получает студента по ID
        public Student GetById(Guid id)
        {
            string sql = "SELECT Id, FullName, BirthDate, GroupId, Email, IsBudget, EnrollmentDate FROM Students WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    // Если есть хотя бы одна строка
                    if (reader.Read())
                    {
                        return new Student
                        {
                            Id = reader.GetGuid(0),
                            FullName = reader.GetString(1),
                            BirthDate = reader.GetDateTime(2),
                            GroupId = reader.GetGuid(3),
                            Email = reader.GetString(4),
                            IsBudget = reader.GetBoolean(5),
                            EnrollmentDate = reader.GetDateTime(6)
                        };
                    }
                }
            }

            // Если не найдено - возвращаем null
            return null;
        }

        // Получает студентов по группе
        public List<Student> GetByGroup(Guid groupId)
        {
            var students = new List<Student>();
            string sql = "SELECT Id, FullName, BirthDate, GroupId, Email, IsBudget, EnrollmentDate FROM Students WHERE GroupId = @GroupId";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@GroupId", groupId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            Id = reader.GetGuid(0),
                            FullName = reader.GetString(1),
                            BirthDate = reader.GetDateTime(2),
                            GroupId = reader.GetGuid(3),
                            Email = reader.GetString(4),
                            IsBudget = reader.GetBoolean(5),
                            EnrollmentDate = reader.GetDateTime(6)
                        });
                    }
                }
            }

            return students;
        }

        // 3. UPDATE - Обновление данных
        // Обновляет данные студента
        public void Update(Student student)
        {
            string sql = @"
                UPDATE Students 
                SET FullName = @FullName, 
                    BirthDate = @BirthDate, 
                    GroupId = @GroupId, 
                    Email = @Email, 
                    IsBudget = @IsBudget, 
                    EnrollmentDate = @EnrollmentDate 
                WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", student.Id);
                command.Parameters.AddWithValue("@FullName", student.FullName);
                command.Parameters.AddWithValue("@BirthDate", student.BirthDate);
                command.Parameters.AddWithValue("@GroupId", student.GroupId);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@IsBudget", student.IsBudget);
                command.Parameters.AddWithValue("@EnrollmentDate", student.EnrollmentDate);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // 4. DELETE - Удаление данных
        // Удаляет студента по ID
        public void Delete(Guid id)
        {
            string sql = "DELETE FROM Students WHERE Id = @Id";

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
