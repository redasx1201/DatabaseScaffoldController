﻿using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Assignment
    {
        public Assignment()
        {
            Submissions = new HashSet<Submission>();
        }

        public int AssignmentNumber { get; set; }
        public int CategoryNumber { get; set; }
        public string Name { get; set; } = null!;
        public uint MaxPoints { get; set; }
        public string? Contents { get; set; }
        public DateTime DueDateTime { get; set; }

        public virtual AssignmentCategory CategoryNumberNavigation { get; set; } = null!;
        public virtual ICollection<Submission> Submissions { get; set; }
    }
}
