using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using PraktikaNORBIT_UniversitySQL_DataBase.Models;


namespace PraktikaNORBIT_UniversitySQL_DataBase.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(string connectionString)  
            : base (connectionString)
        {
            //отключаем LazyLoading - не подгружаем связанные данные автоматически
            // это экономит память и ускоряет работу, если не нужны все связи
            this.Configuration.LazyLoadingEnabled = false;

            //отключаем автоматическое отслеживание изменений
            // увеличивает производительность при чтении данных
            this.Configuration.AutoDetectChangesEnabled = false;

            //отключаем прокси для навигационных свойств
            this.Configuration.ProxyCreationEnabled = false;
        }

        //DbSet - коллекции в памяти, соответствующие таблицам в БД
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Grade> Grades { get; set; }

        //метод для настройки модели. Здесь описываем
        //связи между таблицами, ограничения, индексы и т.д.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //убираем каскадное удаление по умолчанию
            // чтобы случайно не удалить связанные данные
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //НАСТРОЙКА Student
            //Указываем, что Student соответствует таблице Students
            modelBuilder.Entity<Student>().ToTable("Students");

            //указываем первичный ключ(Id)
            modelBuilder.Entity<Student>().HasKey(s => s.Id);

            //настройка FullName, IsRequired - NOT NULL, 
            //HasMaxLength - NVARCHAR(100)
            modelBuilder.Entity<Student>()
                .Property(s => s.FullName)
                .IsRequired().
                HasMaxLength(100);

            //настройка Email
            modelBuilder.Entity<Student>()
                .Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(40);

            //связь Student -> Group (многие к одному)
            // Many: Student, One: Group
            // HasRequired - у студента обязательно есть группа
            // WithMany - у группы может быть много студентов
            // HasForeignKey - внешний ключ GroupId
            // WillCascadeOnDelete(false) - не удалять студента при удалении группы
            modelBuilder.Entity<Student>()
                .HasRequired(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId)
                .WillCascadeOnDelete(false);


            //НАСТРОЙКА Group
            modelBuilder.Entity<Group>()
                .ToTable("Groups")
                .HasKey(g => g.Id);

            modelBuilder.Entity<Group>()
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(20);

            // Связь: Group -> Faculty
            modelBuilder.Entity<Group>()
                .HasRequired(g => g.Faculty)
                .WithMany(f => f.Groups)
                .HasForeignKey(g => g.FacultyId)
                .WillCascadeOnDelete(false);

            // Связь: Group -> Teacher (куратор)
            // HasOptional - куратор может отсутствовать
            modelBuilder.Entity<Group>()
                .HasOptional(g => g.Curator)
                .WithMany()
                .HasForeignKey(g => g.CuratorId)
                .WillCascadeOnDelete(false);


            //НАСТРОЙКА Teacher
            modelBuilder.Entity<Teacher>()
                .ToTable("Teachers")
                .HasKey(t => t.Id);

            modelBuilder.Entity<Teacher>()
                .Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(100);

            // DECIMAL(10,2) - два знака после запятой
            modelBuilder.Entity<Teacher>()
                .Property(t => t.Salary)
                .HasPrecision(10, 2);


            //НАСТРОЙКА Faculty
            modelBuilder.Entity<Faculty>()
                .ToTable("Faculties")
                .HasKey(f => f.Id);

            modelBuilder.Entity<Faculty>()
                .Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Связь: Faculty -> Teacher (декан)
            modelBuilder.Entity<Faculty>()
                .HasRequired(f => f.Dean)
                .WithMany()
                .HasForeignKey(f => f.DeanId)
                .WillCascadeOnDelete(false);


            //НАСТРОЙКА Subject
            modelBuilder.Entity<Subject>()
                .ToTable("Subjects")
                .HasKey(s => s.Id);

            modelBuilder.Entity<Subject>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Subject>()
                .Property(s => s.Credits)
                .HasPrecision(5, 2);


            //НАСТРОЙКА Schedule
            modelBuilder.Entity<Schedule>()
                .ToTable("Schedule")
                .HasKey(s => s.Id);

            // Связь: Schedule -> Group
            modelBuilder.Entity<Schedule>()
                .HasRequired(s => s.Group)
                .WithMany()
                .HasForeignKey(s => s.GroupId)
                .WillCascadeOnDelete(false);

            // Связь: Schedule -> Subject
            modelBuilder.Entity<Schedule>()
                .HasRequired(s => s.Subject)
                .WithMany()
                .HasForeignKey(s => s.SubjectId)
                .WillCascadeOnDelete(false);

            // Связь: Schedule -> Teacher
            modelBuilder.Entity<Schedule>()
                .HasRequired(s => s.Teacher)
                .WithMany()
                .HasForeignKey(s => s.TeacherId)
                .WillCascadeOnDelete(false);


            //НАСТРОЙКА Grade
            modelBuilder.Entity<Grade>()
                .ToTable("Grades")
                .HasKey(g => g.Id);

            modelBuilder.Entity<Grade>()
                .Property(g => g.GradeValue)
                .HasColumnName("Grade");

            // Связь: Grade -> Student
            modelBuilder.Entity<Grade>()
                .HasRequired(g => g.Student)
                .WithMany()
                .HasForeignKey(g => g.StudentId)
                .WillCascadeOnDelete(false);

            // Связь: Grade -> Subject
            modelBuilder.Entity<Grade>()
                .HasRequired(g => g.Subject)
                .WithMany()
                .HasForeignKey(g => g.SubjectId)
                .WillCascadeOnDelete(false);


            // Вызываем базовый метод
            base.OnModelCreating(modelBuilder);
        }
    }
}
