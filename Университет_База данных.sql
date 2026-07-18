CREATE DATABASE University
GO

USE University
GO

CREATE TABLE Students (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), -- уникальный id
    FullName NVARCHAR(100) NOT NULL,                 -- ФИО
    BirthDate DATE NOT NULL,                         -- дата рождения
    GroupId UNIQUEIDENTIFIER NOT NULL,               -- связь с группой
    Email NVARCHAR(40) UNIQUE NOT NULL,              -- строка + уникальный индекс
    IsBudget BIT NOT NULL,                           -- бюджетник/платник
    EnrollmentDate DATE NOT NULL                     -- дата зачисления
);

CREATE TABLE Groups (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(20) NOT NULL UNIQUE,          -- строка, уникальный индекс
    FacultyId UNIQUEIDENTIFIER NOT NULL,        -- связь с факультетом
    Course INT NOT NULL,                        -- курс (1-4)
    CuratorId UNIQUEIDENTIFIER,                 -- связь с преподавателем (куратор)
    Year INT NOT NULL                           -- год поступления
);

CREATE TABLE Teachers (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FullName NVARCHAR(100) NOT NULL,
    BirthDate DATE,
    Department NVARCHAR(100) NOT NULL,           -- кафедра
    AcademicDegree NVARCHAR(50),                 -- ученая степень
    HireDate DATE NOT NULL,                      -- дата найма
    Salary DECIMAL(10,2) NOT NULL                -- зарплата
);

-- Дисциплины
CREATE TABLE Subjects (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    Credits DECIMAL(5,2) NOT NULL,              -- зачетные единицы(1 ЗЕТ = 36 академ.часов)
    Hours INT NOT NULL,                         -- часы
    IsExam BIT NOT NULL,                        -- экзамен или зачет
    Department NVARCHAR(100) NOT NULL           -- кафедра
);

CREATE TABLE Faculties (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL UNIQUE,         -- название
    DeanId UNIQUEIDENTIFIER NOT NULL,           -- связь с преподавателем (декан)
    Phone NVARCHAR(20),                         -- контактный телефон
    Email NVARCHAR(40),                         -- электронная почта
    FoundationDate DATE NOT NULL                -- дата основания
);

-- Расписание
CREATE TABLE Schedule (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    GroupId UNIQUEIDENTIFIER NOT NULL,          -- связь с группой
    SubjectId UNIQUEIDENTIFIER NOT NULL,        -- связь с дисциплиной
    TeacherId UNIQUEIDENTIFIER NOT NULL,        -- связь с преподавателем
    DateTime DATETIME NOT NULL,                 -- дата и время
    Classroom NVARCHAR(10) NOT NULL             -- номер аудитории
);

-- Оценочки :)
CREATE TABLE Grades (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    StudentId UNIQUEIDENTIFIER NOT NULL,
    SubjectId UNIQUEIDENTIFIER NOT NULL,
    Grade INT CHECK (Grade >= 2 AND Grade <= 5),
    Date DATE NOT NULL,
    IsRetake BIT NOT NULL DEFAULT 0
);

-- создание уникального индекса, email не может быть одинаковым
CREATE UNIQUE INDEX idx_students_email ON Students (Email);

-- создание связей между таблицами
ALTER TABLE Faculties
ADD CONSTRAINT FK_Faculties_Dean 
FOREIGN KEY (DeanId) REFERENCES Teachers (Id);

ALTER TABLE Groups 
ADD CONSTRAINT FK_Groups_Faculty 
FOREIGN KEY (FacultyId) REFERENCES Faculties(Id);

ALTER TABLE Groups 
ADD CONSTRAINT FK_Groups_Curator 
FOREIGN KEY (CuratorId) REFERENCES Teachers(Id);

ALTER TABLE Students 
ADD CONSTRAINT FK_Students_Group 
FOREIGN KEY (GroupId) REFERENCES Groups(Id);

ALTER TABLE Schedule 
ADD CONSTRAINT FK_Schedule_Group 
FOREIGN KEY (GroupId) REFERENCES Groups(Id);

ALTER TABLE Schedule 
ADD CONSTRAINT FK_Schedule_Subject 
FOREIGN KEY (SubjectId) REFERENCES Subjects(Id);

ALTER TABLE Schedule 
ADD CONSTRAINT FK_Schedule_Teacher 
FOREIGN KEY (TeacherId) REFERENCES Teachers(Id);

ALTER TABLE Grades 
ADD CONSTRAINT FK_Grades_Student 
FOREIGN KEY (StudentId) REFERENCES Students(Id);

ALTER TABLE Grades 
ADD CONSTRAINT FK_Grades_Subject 
FOREIGN KEY (SubjectId) REFERENCES Subjects(Id);

-- Добавляем преподавателей
INSERT INTO Teachers (FullName, Department, AcademicDegree, HireDate, Salary) VALUES
('Степанова Елена Михайловна', 'Здания, строительные конструкции и материалы', 'Доцент', '2008-11-11', 110000),
('Иванов Иван Иванович', 'Высшая математика', 'Доктор технических наук', '2006-05-16', 150000),
('Петров Петр Петрович', 'Иностранные языки', 'Доцент', '2004-12-24', 100000),
('Сидорова Мария Ивановна', 'Экономика транспорта', 'Кандидат экономических наук', '2010-03-01', 90000);

-- Добавляем факультеты
INSERT INTO Faculties (Name, DeanId, Phone, Email, FoundationDate) VALUES
('Бизнес-информатика',(SELECT Id FROM Teachers WHERE FullName = 'Иванов Иван Иванович'), 
'8-383-328-02-28', 'fbi@university.edu', '2004-09-01'),
('Строительство железных дорог', (SELECT Id FROM Teachers WHERE FullName = 'Петров Петр Петрович'), 
'8-383-328-05-00', 'sgd@stu.ru', '1932-08-08'),
('Промышленное и гражданское строительство', (SELECT Id FROM Teachers WHERE FullName = 'Сидорова Мария Ивановна'), 
'8-800-555-35-35', 'pgs@university.edu', '1957-12-21');

-- Добавляем группы(не крови)
INSERT INTO Groups (Name, FacultyId, Course, Year) VALUES
('БИСТ-211', (SELECT Id FROM Faculties WHERE Name = 'Бизнес-информатика'), 2, 2024),
('БИ-112', (SELECT Id FROM Faculties WHERE Name = 'Бизнес-информатика'), 1, 2025),
('БАУ-301', (SELECT Id FROM Faculties WHERE Name = 'Промышленное и гражданское строительство'), 3, 2023);

-- Добавляем студентиков 
INSERT INTO Students (FullName, BirthDate, GroupId, Email, IsBudget, EnrollmentDate) VALUES
('Климов Матвей Константинович', '2006-04-01', (SELECT Id FROM Groups WHERE Name = 'БИСТ-211'), 
'ya_zadolbalas@mail.ru', 1, '2024-08-05'), 
('Орлова Ольга Владимировна', '2005-07-07', (SELECT Id FROM Groups WHERE Name = 'БИСТ-211'),
'milpops_zhuzhuzhu@yandex.ru', 0, '2024-08-05'),
('Ковалева Евгения Максимовна', '2003-06-29', (SELECT Id FROM Groups WHERE Name = 'БАУ-301'),
'sixseven@gmail.com', 1, '2023-08-06')

-- Добавляем дисциплины, на которые никто не будет ходить(фЕлАсОфЕя)
INSERT INTO Subjects (Name, Credits, Hours, IsExam, Department) VALUES
('Экономика', 2.0, 72, 0, 'Экономика транспорта'),
('Философия', 2.0, 72, 1, 'История и философия'),
('Высшая математика', 4.0, 144, 1, 'Высшая математика'),
('Базы данных', 3.0, 108, 1, 'Кафедра информатики'),
('Иностранный язык', 3.0, 108, 0, 'Кафедра иностранных языков')

-- Добавляем милипусенькое расписание блин блинский
INSERT INTO Schedule (GroupId, SubjectId, TeacherId, DateTime, Classroom) VALUES
((SELECT Id FROM Groups WHERE Name = 'БИСТ-211'),
(SELECT Id FROM Subjects WHERE Name = 'Высшая математика'),
(SELECT Id FROM Teachers WHERE FullName = 'Иванов Иван Иванович'),
'2024-09-02 08:30:00', '410'),

((SELECT Id FROM Groups WHERE Name = 'БИСТ-211'),
(SELECT Id FROM Subjects WHERE Name = 'Иностранный язык'),
(SELECT Id FROM Teachers WHERE FullName = 'Петров Петр Петрович'),
'2024-09-03 10:15:00', '305'),

((SELECT Id FROM Groups WHERE Name = 'БИСТ-211'),
(SELECT Id FROM Subjects WHERE Name = 'Высшая математика'),
(SELECT Id FROM Teachers WHERE FullName = 'Иванов Иван Иванович'),
'2024-09-04 12:00:00', '410'),

((SELECT Id FROM Groups WHERE Name = 'БИСТ-211'),
(SELECT Id FROM Subjects WHERE Name = 'Экономика'),
(SELECT Id FROM Teachers WHERE FullName = 'Сидорова Мария Ивановна'),
'2024-09-04 14:10:00', '202')

-- Добавляем оценочки
INSERT INTO Grades (StudentId, SubjectId, Grade, Date, IsRetake) VALUES
((SELECT Id FROM Students WHERE FullName = 'Климов Матвей Константинович'),
 (SELECT Id FROM Subjects WHERE Name = 'Высшая математика'), 4, '2024-09-05', 0),

((SELECT Id FROM Students WHERE FullName = 'Климов Матвей Константинович'),
 (SELECT Id FROM Subjects WHERE Name = 'Иностранный язык'), 5, '2024-09-06', 0),

((SELECT Id FROM Students WHERE FullName = 'Орлова Ольга Владимировна'),
 (SELECT Id FROM Subjects WHERE Name = 'Высшая математика'), 3, '2024-09-05', 0),

((SELECT Id FROM Students WHERE FullName = 'Орлова Ольга Владимировна'),
 (SELECT Id FROM Subjects WHERE Name = 'Экономика'), 4, '2024-09-07', 0),

((SELECT Id FROM Students WHERE FullName = 'Ковалева Евгения Максимовна'),
 (SELECT Id FROM Subjects WHERE Name = 'Базы данных'), 5, '2024-09-08', 0);

-- 1. Выборка данных с фильтрацией, сортировкой
-- Все студенты группы БИСТ-211 (отсортированы по ФИО)
SELECT 
    FullName AS 'ФИО',
    BirthDate AS 'Дата рождения',
    Email AS 'Почта',
    CASE 
        WHEN IsBudget = 1 THEN 'Бюджет'
        ELSE 'Платник'
    END AS 'Форма обучения'
FROM Students
WHERE GroupId = (SELECT Id FROM Groups WHERE Name = 'БИСТ-211')
ORDER BY FullName;

-- 2. Удаление, изменение данных
-- Назначаем препода деканом
UPDATE Faculties 
SET DeanId = (SELECT Id FROM Teachers WHERE FullName = 'Степанова Елена Михайловна')
WHERE Name = 'Промышленное и гражданское строительство';

-- Назначаем препода куратором
UPDATE Groups 
SET CuratorId = (SELECT Id FROM Teachers WHERE FullName = 'Степанова Елена Михайловна')
WHERE Name = 'БАУ-301';

-- Увеличить зарплату всем преподавателям на 10%
UPDATE Teachers 
SET Salary = Salary * 1.1;

-- Удаление студента (например, отчисленного)
DELETE FROM Students 
WHERE FullName = 'Климов Матвей Константинович';

-- 3. Выборка с группировкой
-- Количество студентов по группам
SELECT 
    g.Name AS 'Группа',
    COUNT(*) AS 'Количество студентов'
FROM Students s
JOIN Groups g ON s.GroupId = g.Id
GROUP BY g.Name
ORDER BY COUNT(*) DESC;

-- 4. Выборка из нескольких связанных таблиц (левое, правое соединение, пересечение)
-- Левое соединение: все группы и их кураторы (даже если нет куратора)
SELECT 
    g.Name AS 'Группа',
    t.FullName AS 'Куратор'
FROM Groups g
LEFT JOIN Teachers t ON g.CuratorId = t.Id
ORDER BY g.Name;

-- Правое соединение: все преподаватели и их группы (даже если нет группы)
SELECT 
    t.FullName AS 'Преподаватель',
    g.Name AS 'Курируемая группа'
FROM Groups g
RIGHT JOIN Teachers t ON g.CuratorId = t.Id
ORDER BY t.FullName;

-- Пересечение: преподаватели, которые являются и деканами, и кураторами
SELECT t.FullName AS 'Преподаватель'
FROM Teachers t
JOIN Faculties f ON t.Id = f.DeanId
INTERSECT
SELECT t.FullName
FROM Teachers t
JOIN Groups g ON t.Id = g.CuratorId;

-- Проверка связи N:N
SELECT 
    s.FullName AS 'Студент',
    sub.Name AS 'Дисциплина',
    gr.Grade AS 'Оценка'
FROM Students s
JOIN Grades gr ON s.Id = gr.StudentId
JOIN Subjects sub ON gr.SubjectId = sub.Id
ORDER BY s.FullName;