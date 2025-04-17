using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Enrolled
    {
        public int EnrollmentNumber { get; set; }
        public int StudentNumber { get; set; }
        public int CourseNumber { get; set; }
        public string Grade { get; set; } = null!;

        public virtual Course CourseNumberNavigation { get; set; } = null!;
        public virtual Student StudentNumberNavigation { get; set; } = null!;
    }
}
