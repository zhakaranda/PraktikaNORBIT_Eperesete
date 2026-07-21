using System;
using System.Configuration;
using System.Linq;
using PraktikaNORBIT_UniversitySQL_DataBase.Data;
using PraktikaNORBIT_UniversitySQL_DataBase.Data.Repositories;
using PraktikaNORBIT_UniversitySQL_DataBase.Models;

namespace PraktikaNORBIT_UniversitySQL_DataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. ПОЛУЧАЕМ СТРОКУ ПОДКЛЮЧЕНИЯ ИЗ КОНФИГА
            // Читаем строку подключения из App.config
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            Console.WriteLine(" УНИВЕРСИТЕТ: РАБОТА С ДАННЫМИ ");
            Console.WriteLine();

            // 2. ДЕМОНСТРАЦИЯ ADO.NET

            Console.WriteLine("  ЧАСТЬ 1: РАБОТА ЧЕРЕЗ ADO.NET");
            Console.WriteLine();

            // 2.1 Студенты
            Console.WriteLine("--- СТУДЕНТЫ ---");
            var studentRepo = new StudentRepository(connectionString);

            // CREATE - добавляем студента
            var newStudent = new Student
            {
                Id = Guid.NewGuid(),
                FullName = "Белоусов Иван Львович",
                BirthDate = new DateTime(2007, 11, 1),
                GroupId = GetFirstGroupId(connectionString),
                Email = "belous@mail.ru",
                IsBudget = true,
                EnrollmentDate = DateTime.Now
            };
            studentRepo.Add(newStudent);
            Console.WriteLine($"Добавлен студент: {newStudent.FullName}");

            // READ - получаем всех студентов
            var students = studentRepo.GetAll();
            Console.WriteLine($"Всего студентов: {students.Count}");

            // UPDATE - обновляем студента
            if (students.Count > 0)
            {
                var firstStudent = students[0];
                string oldName = firstStudent.FullName;
                firstStudent.FullName = "Обновленный Студент";
                studentRepo.Update(firstStudent);
                Console.WriteLine($"Обновлён студент: {oldName} → {firstStudent.FullName}");
            }

            // 2.2 Группы
            Console.WriteLine("\n--- ГРУППЫ ---");
            var groupRepo = new GroupRepository(connectionString);
            var groups = groupRepo.GetAll();
            Console.WriteLine($"Всего групп: {groups.Count}");

            // 2.3 Преподаватели
            Console.WriteLine("\n--- ПРЕПОДАВАТЕЛИ ---");
            var teacherRepo = new TeacherRepository(connectionString);
            var teachers = teacherRepo.GetAll();
            Console.WriteLine($"Всего преподавателей: {teachers.Count}");

            // 2.4 Дисциплины
            Console.WriteLine("\n--- ДИСЦИПЛИНЫ ---");
            var subjectRepo = new SubjectRepository(connectionString);
            var subjects = subjectRepo.GetAll();
            Console.WriteLine($"Всего дисциплин: {subjects.Count}");

            // 2.5 Факультеты
            Console.WriteLine("\n--- ФАКУЛЬТЕТЫ ---");
            var facultyRepo = new FacultyRepository(connectionString);
            var faculties = facultyRepo.GetAll();
            Console.WriteLine($"Всего факультетов: {faculties.Count}");

            // 2.6 Расписание
            Console.WriteLine("\n--- РАСПИСАНИЕ ---");
            var scheduleRepo = new ScheduleRepository(connectionString);
            var schedules = scheduleRepo.GetAll();
            Console.WriteLine($"Всего занятий: {schedules.Count}");

            // 2.7 Оценки
            Console.WriteLine("\n--- ОЦЕНКИ ---");
            var gradeRepo = new GradeRepository(connectionString);
            var grades = gradeRepo.GetAll();
            Console.WriteLine($"Всего оценок: {grades.Count}");

            // DELETE - удаляем созданного студента (комментируем, чтобы случайно не удалить)
            //studentRepo.Delete(newStudent.Id);
            //Console.WriteLine($"Удалён студент: {newStudent.FullName}");

            // 3. ДЕМОНСТРАЦИЯ ENTITY FRAMEWORK
            Console.WriteLine();
            Console.WriteLine("  ЧАСТЬ 2: РАБОТА ЧЕРЕЗ ENTITY FRAMEWORK");
            Console.WriteLine();

            using (var context = new UniversityContext(connectionString))
            {
                // 3.1 Количество записей
                Console.WriteLine("--- СТАТИСТИКА ---");
                Console.WriteLine($"Студентов: {context.Students.Count()}");
                Console.WriteLine($"Групп: {context.Groups.Count()}");
                Console.WriteLine($"Преподавателей: {context.Teachers.Count()}");
                Console.WriteLine($"Дисциплин: {context.Subjects.Count()}");
                Console.WriteLine($"Факультетов: {context.Faculties.Count()}");
                Console.WriteLine($"Занятий: {context.Schedules.Count()}");
                Console.WriteLine($"Оценок: {context.Grades.Count()}");

                // 3.2 CREATE - добавляем студента через EF
                Console.WriteLine("\n--- ДОБАВЛЕНИЕ (EF) ---");
                var efStudent = new Student
                {
                    Id = Guid.NewGuid(),
                    FullName = "EF Студент2",
                    BirthDate = new DateTime(2006, 1, 13),
                    GroupId = GetFirstGroupId(connectionString),
                    Email = "ef2.student@mail.ru",
                    IsBudget = false,
                    EnrollmentDate = DateTime.Now
                };
                context.Students.Add(efStudent);
                context.SaveChanges();
                Console.WriteLine($"Добавлен студент через EF: {efStudent.FullName}");

                // 3.3 UPDATE - обновляем через EF
                Console.WriteLine("\n--- ОБНОВЛЕНИЕ (EF) ---");
                var studentToUpdate = context.Students.FirstOrDefault(s => s.FullName == "EF Студент");
                if (studentToUpdate != null)
                {
                    studentToUpdate.FullName = "Обновлённый EF Студент";
                    context.SaveChanges();
                    Console.WriteLine($"Обновлён студент через EF: {studentToUpdate.FullName}");
                }

                // 3.4 DELETE - удаляем через EF
                // Console.WriteLine("\n--- УДАЛЕНИЕ (EF) ---");
                // var studentToDelete = context.Students.FirstOrDefault(s => s.FullName == "Обновлённый EF Студент");
                // if (studentToDelete != null)
                // {
                //     context.Students.Remove(studentToDelete);
                //     context.SaveChanges();
                //     Console.WriteLine($"🗑️ Удалён студент через EF: {studentToDelete.FullName}");
                // }

                // 3.5 СЛОЖНЫЙ ЗАПРОС - студенты с их оценками
                Console.WriteLine("\n--- СТУДЕНТЫ И ОЦЕНКИ (LINQ) ---");

                var studentsWithGrades = context.Students
                    .Select(s => new
                    {
                        StudentName = s.FullName,
                        Grades = context.Grades
                            .Where(g => g.StudentId == s.Id)
                            .Select(g => g.GradeValue)
                            .ToList()
                    })
                    .ToList();

                foreach (var item in studentsWithGrades)
                {
                    string gradesStr = item.Grades.Any()
                        ? string.Join(", ", item.Grades)
                        : "нет оценок";
                    Console.WriteLine($"  {item.StudentName}: [{gradesStr}]");
                }

                // 3.6 Группировка - средний балл по группам
                Console.WriteLine("\n--- СРЕДНИЙ БАЛЛ ПО ГРУППАМ ---");

                var groupAverageGrades = context.Students
                    .Join(context.Grades, s => s.Id, g => g.StudentId, (s, g) => new { s, g })
                    .Join(context.Groups, sg => sg.s.GroupId, gr => gr.Id, (sg, gr) => new { sg.s, sg.g, gr })
                    .GroupBy(x => x.gr.Name)
                    .Select(g => new
                    {
                        GroupName = g.Key,
                        AvgGrade = g.Average(x => x.g.GradeValue)
                    })
                    .ToList();

                foreach (var item in groupAverageGrades)
                {
                    Console.WriteLine($"  {item.GroupName}: средний балл {item.AvgGrade:F2}");
                }
            }

            Console.WriteLine();
            Console.WriteLine(" ПРОГРАММА ЗАВЕРШЕНА ");
            Console.ReadKey();
        }

        // ============================================
        // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
        // ============================================

        // Получает ID первой группы из БД (или создаёт, если нет)
        private static Guid GetFirstGroupId(string connectionString)
        {
            using (var context = new UniversityContext(connectionString))
            {
                var group = context.Groups.FirstOrDefault();
                if (group != null)
                    return group.Id;

                // Если группы нет, создаём тестовую
                var faculty = context.Faculties.FirstOrDefault();
                if (faculty == null)
                {
                    // Если факультета нет, создаём тестовый
                    var teacher = context.Teachers.FirstOrDefault();
                    if (teacher == null)
                    {
                        // Если преподавателя нет, создаём
                        var newTeacher = new Teacher
                        {
                            Id = Guid.NewGuid(),
                            FullName = "Тестовый Преподаватель",
                            Department = "Тестовая кафедра",
                            HireDate = DateTime.Now.AddYears(-5),
                            Salary = 100000
                        };
                        context.Teachers.Add(newTeacher);
                        context.SaveChanges();
                        teacher = newTeacher;
                    }

                    var newFaculty = new Faculty
                    {
                        Id = Guid.NewGuid(),
                        Name = "Тестовый Факультет",
                        DeanId = teacher.Id,
                        FoundationDate = DateTime.Now.AddYears(-10)
                    };
                    context.Faculties.Add(newFaculty);
                    context.SaveChanges();
                    faculty = newFaculty;
                }

                var newGroup = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "ТЕСТ-001",
                    FacultyId = faculty.Id,
                    Course = 1,
                    Year = DateTime.Now.Year
                };
                context.Groups.Add(newGroup);
                context.SaveChanges();
                return newGroup.Id;
            }
        }
    }
}
