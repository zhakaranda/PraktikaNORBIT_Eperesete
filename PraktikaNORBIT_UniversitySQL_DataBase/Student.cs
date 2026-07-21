using System;


namespace PraktikaNORBIT_UniversitySQL_DataBase.Models
{
    public class Student
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public Guid GroupId { get; set; }
        public string Email { get; set; }
        public bool IsBudget { get; set; }
        public DateTime EnrollmentDate { get; set; }

        // Навигационное свойство (связь с группой)
        public virtual Group Group { get; set; }
    }
}
