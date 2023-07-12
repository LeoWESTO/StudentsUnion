﻿using StudentsUnion.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentsUnion.ViewModels
{
    public class BidViewModel
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
