using System;


namespace PraktikaNORBIT_UniversitySQL_DataBase.Models
{
    public class Teacher
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Department { get; set; }
        public string AcademicDegree { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
    }
}
