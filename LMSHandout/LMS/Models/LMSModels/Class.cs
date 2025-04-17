using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Class
    {
        public int CourseNumber { get; set; }
        public uint Year { get; set; }
        public string Season { get; set; } = null!;
        public string Location { get; set; } = null!;
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public virtual Course CourseNumberNavigation { get; set; } = null!;
    }
}
