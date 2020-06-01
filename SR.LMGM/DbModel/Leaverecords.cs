using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SR.LMGM.DbModel
{
    public partial class Leaverecords
    {
        public int Id { get; set; }
        public string Reqid { get; set; }
        public string Empcode { get; set; }
        [DataType(DataType.MultilineText)]
        public string Reason { get; set; }
        public int Leavetype { get; set; }
        [DataType(DataType.Date)]
        public DateTime Leavefrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime Leaveto { get; set; }
        public string Assignedto { get; set; }
        public int Status { get; set; }
        public string Lastapprover { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Lastapprovaltime { get; set; }
        [DataType(DataType.MultilineText)]
        public string Rejectiondesc { get; set; }
        public string Attachmentpath { get; set; }
    }
}
