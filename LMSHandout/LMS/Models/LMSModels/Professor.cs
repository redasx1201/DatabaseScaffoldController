﻿using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Professor
    {
        public string UId { get; set; } = null!;
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public DateOnly Dob { get; set; }
        public string WorksIn { get; set; } = null!;
    }
}
