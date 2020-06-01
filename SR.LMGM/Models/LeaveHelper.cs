using SR.LMGM.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SR.LMGM.Models
{
    public class LeaveHelper
    {
        public string reqId { get; set;}
        public string currentapprover { get; set; }
        public string previousapprover { get; set; }
        public string teamname { get; set; }
        public string empname { get; set; }
        public string empcode { get; set; }
        public string leavecategory { get; set; }

        public string status { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
    }
    
    public class LeaveRejectData
    {
        public string reqId { get; set; }
        public string rejectDesc { get; set; }
    }

    public class LeaveProvider
    {
        public static string GetMailAddress(string empCode)
        {
            MySqlContext db = new MySqlContext();
            var email = db.Users.Where(x => x.Empcode.Equals(empCode)).Select(x=>x.Email).FirstOrDefault();
            return email;
        }

        public static List<LeaveHelper> GetManagerLeaveList(Users user)
        {
            MySqlContext db = new MySqlContext();

            List<LeaveHelper> leaveList = db.Leaverecords.Where(x => x.Assignedto.Equals(user.Empcode)).Select(x => new LeaveHelper
            {
                currentapprover = GetEmpName(x.Assignedto),
                previousapprover = GetEmpName(x.Lastapprover),
                from = x.Leavefrom,
                to = x.Leaveto,
                leavecategory = GetEnumDesc(x.Leavetype),
                empname = GetEmpName(x.Empcode),
                empcode = x.Empcode,
                teamname = GetTeamName(x.Empcode),
                reqId = x.Reqid,
                status = GetEnumDesc(x.Status)
            }).ToList();

            return leaveList;
        }

        public static List<LeaveHelper> GetHrLeaveList(Users user)
        {
            MySqlContext db = new MySqlContext();

            List<LeaveHelper> leaveList = db.Leaverecords.Where(x => x.Assignedto.Equals(user.Empcode)).Select(x => new LeaveHelper
            {
                currentapprover = GetEmpName(x.Assignedto),
                previousapprover = GetEmpName(x.Lastapprover),
                from = x.Leavefrom,
                to = x.Leaveto,
                leavecategory = GetEnumDesc(x.Leavetype),
                empname = GetEmpName(x.Empcode),
                empcode = x.Empcode,
                teamname = GetTeamName(x.Empcode),
                reqId = x.Reqid,
                status = GetEnumDesc(x.Status)
            }).ToList();

            return leaveList;
        }

        public static string GetEmpemail(string empcode)
        {
            MySqlContext db = new MySqlContext();
            var email = db.Users.Where(x => x.Empcode.Equals(empcode)).Select(x => x.Email).FirstOrDefault();
            return email;
        }

        public static string GetAssigneeEmail(string empcode)
        {
            MySqlContext db = new MySqlContext();
            var email = db.Users.Where(x => x.Empcode.Equals(empcode)).Select(x => x.Email).FirstOrDefault();
            return email;
        }

        public static string GetEmpName(string empcode)
        {
            MySqlContext db = new MySqlContext();
            var name = db.Users.Where(x => x.Empcode.Equals(empcode)).Select(x => x.Empname).FirstOrDefault();
            return name;
        }
        public static string GetTeamName(string empcode)
        {
            MySqlContext db = new MySqlContext();
            var name = db.Users.Where(x => x.Empcode.Equals(empcode)).Select(x => x.Teamname).FirstOrDefault();
            return name;
        }

        public static string GetEmpCodeForTeamsHr(string team)
        {
            MySqlContext db = new MySqlContext();
            var name = db.Users.Where(x => x.Teamname.Equals(team) && x.Designation.Equals(100003)).Select(x => x.Empcode).FirstOrDefault();
            return name;
        }

        public static string GetEnumDesc(int code)
        {
            MySqlContext db = new MySqlContext();
            var name = db.Enumvals.Where(x => x.Code.Equals(code)).Select(x => x.Desc).First();
            return name;
        }
    }
}