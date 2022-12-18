﻿using System;

namespace MyProjectUniversityPanel.Models
{
    public class Outcome
    {
        public int Id { get; set; }
        public int Money { get; set; }
        public string For { get; set; }
        public DateTime Date { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public bool IsDeactive { get; set; }

    }
}
