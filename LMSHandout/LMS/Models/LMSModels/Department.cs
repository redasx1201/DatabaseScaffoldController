using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Department
    {
        public Department()
        {
            Courses = new HashSet<Course>();
            Students = new HashSet<Student>();
        }

        public string SubjectAbbreviation { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
