using System;
using System.Collections.Generic;

namespace SR.LMGM.DbModel
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Empcode { get; set; }
        public string Empname { get; set; }
        public string Empcont { get; set; }
        public string Email { get; set; }
        public int Designation { get; set; }
        public string Managedby { get; set; }
        public string Teamname { get; set; }
    }
}
