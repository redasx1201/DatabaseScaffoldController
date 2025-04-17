using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Submission
    {
        public int SubmissionNumber { get; set; }
        public int AssignmentNumber { get; set; }
        public int StudentNumber { get; set; }
        public DateTime SubmittedAt { get; set; }
        public uint Score { get; set; }
        public string? Contents { get; set; }

        public virtual Assignment AssignmentNumberNavigation { get; set; } = null!;
        public virtual Student StudentNumberNavigation { get; set; } = null!;
    }
}
