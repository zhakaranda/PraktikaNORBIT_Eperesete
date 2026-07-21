using System;


namespace PraktikaNORBIT_UniversitySQL_DataBase.Models
{
    public class Subject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Credits { get; set; }
        public int Hours { get; set; }
        //true - экзамен, false - зачёт
        public bool IsExam { get; set; }
        public string Department { get; set; }
    }
}
