using SR.LMGM.DbModel;
using SR.LMGM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace SR.LMGM.Controllers
{
    public class HomeController : Controller
    {
        private MySqlContext db = new MySqlContext();

        const string SessionName = "session";
        // GET: Home
        public ActionResult Index()
        {
            if (TempData["Users"] != null)
            {//set the session
                HttpContext.Session[SessionName] = (Users)TempData["Users"];
            }
            Users user = (Users)HttpContext.Session[SessionName];
            ViewBag.Username = user.Username;
            ViewBag.Userdesg = user.Empcode;
            if (!string.IsNullOrEmpty(ViewBag.Username))
            {
                if (user.Designation.Equals(100005))
                {
                    var leaverequests = db.Leaverecords.Where(x => x.Empcode.Equals(user.Empcode));
                    return View(leaverequests);
                }
                else if (user.Designation.Equals(100003))
                {
                    var leaverequests = LeaveProvider.GetHrLeaveList(user);
                    return View("Approval", leaverequests);
                }
                else
                {
                    var leaverequests = LeaveProvider.GetManagerLeaveList(user);
                    return View("Manager", leaverequests);
                }
            }
            else
            {
                RedirectToAction("Login", "Users");
            }
            return View();
        }

        public ActionResult Approve(string id)
        {
            try
            {
                Users user = (Users)HttpContext.Session[SessionName];
                if (user.Designation.Equals(100004))//approved by manager
                {
                    Leaverecords lv = db.Leaverecords.Where(x => x.Reqid.Equals(id)).FirstOrDefault();
                    lv.Status = 300002;
                    lv.Assignedto = LeaveProvider.GetEmpCodeForTeamsHr(user.Teamname); //when manager approaves the assignment will go to teams HR 
                    lv.Lastapprover = user.Empcode;
                    lv.Lastapprovaltime = DateTime.Now;
                    db.Leaverecords.Update(lv);
                    db.SaveChanges();
                    Helper.SendApprovalMail(lv, 100004, user);
                }
                else if (user.Designation.Equals(100003))//approved by hr
                {
                    Leaverecords lv = db.Leaverecords.Where(x => x.Reqid.Equals(id)).FirstOrDefault();
                    lv.Status = 300004;
                    lv.Lastapprover = user.Empcode;
                    lv.Lastapprovaltime = DateTime.Now;
                    db.Leaverecords.Update(lv);
                    db.SaveChanges();
                    Helper.SendApprovalMail(lv, 100003, user);
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Reject(string reqId)
        {
            ViewBag.ReqId = reqId;
            return PartialView();
        }

        [HttpPost]
        public ActionResult Reject(LeaveRejectData rejectData)
        {
            try
            {
                string id = rejectData.reqId;
                Users user = (Users)HttpContext.Session[SessionName];
                if (user.Designation.Equals(100004))//rejected by manager
                {
                    Leaverecords lv = db.Leaverecords.Where(x => x.Reqid.Equals(id)).FirstOrDefault();
                    lv.Status = 300003;
                    lv.Rejectiondesc = rejectData.rejectDesc;
                    db.Leaverecords.Update(lv);
                    db.SaveChanges();
                    Helper.SendRejectMail(lv, 100004, user);
                }
                else if (user.Designation.Equals(100003))//rejected by hr
                {
                    Leaverecords lv = db.Leaverecords.Where(x => x.Reqid.Equals(id)).FirstOrDefault();
                    lv.Status = 300005;
                    lv.Rejectiondesc = rejectData.rejectDesc;
                    db.Leaverecords.Update(lv);
                    db.SaveChanges();
                    Helper.SendRejectMail(lv, 100003, user);
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public ActionResult Create()
        {
            ViewBag.ReqId = Common.GetNextReqId();
            Users user = (Users)HttpContext.Session[SessionName];
            Leaverecords lv = new Leaverecords { Reqid = Common.GetNextReqId(), Empcode = user.Empcode };
            lv.Assignedto = user.Managedby;// submission directs to the manager.
            ViewBag.LeaveTypes = Common.GetLeaveOptions();
            return View(lv);
        }

        // POST: Requirements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Leaverecords leave, HttpPostedFileBase filename)
        {
            if (ModelState.IsValid)
            {
                leave.Status = 300001;//default due status is inserted
                Users user = (Users)HttpContext.Session[SessionName];
                leave.Assignedto = user.Managedby;// submission directs to the manager.
                leave.Attachmentpath = Helper.SaveUploadedFile(filename);
                db.Add(leave);
                await db.SaveChangesAsync();
                Helper.SendLeaveApplicationMail(LeaveProvider.GetMailAddress(user.Managedby), user, "Leave application",leave.Reqid);
                return RedirectToAction(nameof(Index));
            }
            return View(leave);
        }

    }
}