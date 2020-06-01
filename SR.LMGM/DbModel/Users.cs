using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SR.LMGM.DbModel
{
    public partial class Users
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Empcode { get; set; }
        public string Empname { get; set; }
        public string Empcont { get; set; }
        public int Designation { get; set; }
        public string Managedby { get; set; }
    }
}
