using SR.LMGM.DbModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SR.LMGM.Models
{
    public class Common
    {
        public static SelectList GetLeaveType()
        {
            MySqlContext db = new MySqlContext();
            var leaveTypes = (from s in db.Enumvals where s.Type == 200 select new SelectListItem { Value = s.Code.ToString(), Text = s.Desc });
            SelectList sl = new SelectList(leaveTypes);
            return sl;
        }

        public static IEnumerable<SelectListItem> GetLeaveOptions()
        {
            MySqlContext db = new MySqlContext();
            IEnumerable<SelectListItem> leaveTypes = (from s in db.Enumvals where s.Type == 200 select new SelectListItem { Value = s.Code.ToString(), Text = s.Desc });
            return leaveTypes;
        }

        public static string GetNextReqId()
        {
            MySqlContext db = new MySqlContext();
            int max = db.Leaverecords.Count();
            string reqId = "PP-" + (1000 + max).ToString();
            return reqId;
        }
    }


    public class SelectStatus
    {
        [Key]
        public int Id { get; set; }
        public string Desc { get; set; }
    }

}