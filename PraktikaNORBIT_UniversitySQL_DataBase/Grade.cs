using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace PraktikaNORBIT_UniversitySQL_DataBase.Models
{
    public class Grade
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        [Column("Grade")]
        public int GradeValue { get; set; }  // 2, 3, 4, 5
        public DateTime Date { get; set; }
        public bool IsRetake { get; set; }

        // Навигационные свойства (связи)
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
