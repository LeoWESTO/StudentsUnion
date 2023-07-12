﻿namespace StudentsUnion.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public string Role { get; set; }
    }
}
