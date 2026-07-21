using System;
using System.Collections.Generic;


namespace PraktikaNORBIT_UniversitySQL_DataBase.Models
{
    public class Faculty
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DeanId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime FoundationDate { get; set; }

        // Навигационные свойства
        public virtual Teacher Dean { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
