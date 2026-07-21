using System;
using System.Collections.Generic;


namespace PraktikaNORBIT_UniversitySQL_DataBase.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid FacultyId { get; set; }
        public int Course { get; set; }
        //? - свойство может быть null
        public Guid? CuratorId { get; set; }
        public int Year { get; set; }

        // Навигационные свойства
        public virtual Faculty Faculty { get; set; }
        //коллекция студентов для связи "один ко многим"
        public virtual ICollection<Student> Students { get; set; }
        public virtual Teacher Curator { get; set; }
    }
}
