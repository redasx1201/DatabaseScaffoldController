﻿using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Assignment
    {
        public uint AssignmentId { get; set; }
        public string Name { get; set; } = null!;
        public string Contents { get; set; } = null!;
        public DateTime Due { get; set; }
        public uint MaxPoints { get; set; }
        public uint Category { get; set; }
    }
}
