using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PraktikaNORBIT_UniversitySQL_DataBase.Models
{
    public class Schedule
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid TeacherId { get; set; }
        public DateTime DateTime { get; set; }
        public string Classroom { get; set; }

        // Навигационные свойства (связи)
        public virtual Group Group { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
