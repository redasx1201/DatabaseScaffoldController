using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Course
    {
        public Course()
        {
            Classes = new HashSet<Class>();
            Enrolleds = new HashSet<Enrolled>();
        }

        public int CourseNumber { get; set; }
        public string SubjectAbbreviation { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Number { get; set; }

        public virtual Department SubjectAbbreviationNavigation { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Enrolled> Enrolleds { get; set; }
    }
}
