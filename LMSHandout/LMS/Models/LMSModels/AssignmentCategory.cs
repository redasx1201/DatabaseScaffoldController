using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class AssignmentCategory
    {
        public AssignmentCategory()
        {
            Assignments = new HashSet<Assignment>();
        }

        public int CategoryNumber { get; set; }
        public int CourseNumber { get; set; }
        public string Name { get; set; } = null!;
        public uint GradingWeight { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
