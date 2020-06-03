using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using SR.LMGM.DbModel;

namespace SR.LMGM.Models
{
    public static class Helper
    {
        #region Database Connection String

        private static string _srConnetion;

        public static string SRConnection
        {
            get
            {
                return _srConnetion ?? (_srConnetion = ConfigurationManager.ConnectionStrings["SRDB"].ToString());
            }
            set
            {
                _srConnetion = value;
            }
        }

        #endregion Database Connection String

        public static string GetHash(this string refreshTokenId)
        {
            HashAlgorithm hashalgorithm = new SHA256CryptoServiceProvider();
            byte[] bytevalue = System.Text.Encoding.UTF8.GetBytes(refreshTokenId);
            byte[] bytehash = hashalgorithm.ComputeHash(bytevalue);
            return Convert.ToBase64String(bytehash);
        }

        #region Encrypt

        public const string EncryptionKey = "OPV";

        public static string ToDecrypt(this object cypherText)
        {
            string cText = cypherText.ToString();
            string dyCryptText = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(cText))
                {
                    cText = cText.Replace('-', '+').Replace('_', '/').Replace('$', '=');
                    byte[] b = Convert.FromBase64String(cText);
                    TripleDES des = CreateDes(EncryptionKey);
                    ICryptoTransform ct = des.CreateDecryptor();
                    byte[] output = ct.TransformFinalBlock(b, 0, b.Length);
                    dyCryptText = Encoding.Unicode.GetString(output);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dyCryptText;
        }

        public static string ToEncrypt(this object plainText)
        {
            string pText = plainText.ToString();
            string encrptText = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(pText))
                {
                    TripleDES des = CreateDes(EncryptionKey);
                    ICryptoTransform ct = des.CreateEncryptor();
                    byte[] input = Encoding.Unicode.GetBytes(pText);
                    encrptText = Convert.ToBase64String(ct.TransformFinalBlock(input, 0, input.Length));
                    encrptText = encrptText.Replace('+', '-').Replace('/', '_').Replace('=', '$');
                }
            }
            catch (Exception)
            {
                throw;
            }

            return encrptText;
        }

        internal static void SendApprovalMail(Leaverecords record, int designation, Users user)
        {
            MySqlContext db = new MySqlContext();
            if (designation.Equals(100004)) // manager to hr manager to employee
            {
                //Mail to employee 
                string mgrtoemp = PopulateEmpRespFromMgr(user.Empname, LeaveProvider.GetEmpName(record.Empcode), LeaveProvider.GetEmpName(record.Assignedto), record.Reqid);
                SendEmail(LeaveProvider.GetEmpemail(record.Empcode), mgrtoemp, "Leave accepted by manager - "+record.Reqid);

                string mgrtohr = PopulateHrRespFromMgr(LeaveProvider.GetEmpName(record.Assignedto), user.Empname, record.Reqid);
                SendEmail(LeaveProvider.GetEmpemail(record.Assignedto), mgrtohr, "New leave request received for approval - " + record.Reqid);
            }
            else if (designation.Equals(100003)) // hr approving the mail.
            {
                string hrtoemp = PopulateEmpRespFromHr(LeaveProvider.GetEmpName(record.Empcode), user.Empname, record.Reqid);
                SendEmail(LeaveProvider.GetEmpemail(record.Empcode), hrtoemp, "Leave accepted by manager - " + record.Reqid);
            }

        }

        public static TripleDES CreateDes(string Key)
        {
            TripleDES des = new TripleDESCryptoServiceProvider();
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(Key));
                des.IV = new byte[des.BlockSize / 8];
            }
            catch (Exception)
            {
                throw;
            }
            return des;
        }

        internal static void SendRejectMail(Leaverecords record, int v, Users user)
        {
            string rejectingUser = user.Empname;
            string rejLeaveAppln = LeaveProvider.GetEmpName(record.Empcode);
            string body = PopulateEmpRespForReject(rejectingUser, rejLeaveAppln, record.Reqid);
            SendEmail(LeaveProvider.GetEmpemail(record.Empcode), body, "Leave rejected " + record.Reqid);
        }

        private static string PopulateEmpRespForReject(string rejectingUser, string rejLeaveAppln, string reqid)
        {
            string body = string.Empty;
            body += "Hi " + rejLeaveAppln + ", <br />";
            body += "Your request (" + reqid + ") has been rejected by " + rejectingUser + "<br />";
            return body;
        }

        /// <summary>
        /// file upload function
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        internal static string SaveUploadedFile(HttpPostedFileBase file)
        {
            string _path = "";
            try
            {

                if(file.ContentLength > 0)
                {
                    string _fileName = Path.GetFileName(file.FileName);
                    _path = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), _fileName);
                    file.SaveAs(_path);
                }
                return _path;
            }
            catch(Exception ex)
            {
                return _path;
            }
        }

        #endregion Encrypt

        #region SMTP settings

        //public static bool SendEmail(string mailTo, string password, string subjectTitle)
        //{
        //    try
        //    {
        //        MailMessage mail = new MailMessage();
        //        SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"]);
        //        mail.From = new MailAddress(ConfigurationManager.AppSettings["smtpUser"]);
        //        mail.To.Add(mailTo);
        //        mail.Subject = subjectTitle;
        //        mail.Body = string.Format(PopulateBody(mailTo, password));
        //        mail.IsBodyHtml = true;
        //        SmtpServer.Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
        //        SmtpServer.UseDefaultCredentials = false;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtpUser"], ConfigurationManager.AppSettings["smtpPass"]);
        //        SmtpServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
        //        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        SmtpServer.Send(mail);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public static void SendEmail(string mailTo, string Empname,string managername, string requestcode, string subjectTitle)
        {
            var fromAddress = "leaveapp2020@gmail.com";

            // any address where the email will be sending
            var toAddress = mailTo;
            //Password of your gmail address
            const string fromPassword = "abc123AB";
            // Passing the values and make a email formate to display
            string subject = "Leave Application From " + Empname;
            string body = "";
            body += "Hi " + managername + ", <br />";
            body += "You have recieved a leave application from " + Empname + ". <br />";
            body += "Please go through the request code : " + requestcode;
            // smtp settings

            MailMessage mail = new MailMessage();
            mail.To.Add(new MailAddress(toAddress));
            mail.From = new MailAddress(fromAddress);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(fromAddress, fromPassword); // Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(mail);
        }

        public static bool SendEmail(string mailTo, string body, string subTitle)
        {
            try
            {

                string fromAddress = ConfigurationManager.AppSettings["smtpUser"];
                string password = ConfigurationManager.AppSettings["smtpPass"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
                bool enablessl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromAddress);
                mail.To.Add(mailTo);
                mail.Subject = subTitle;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Host = ConfigurationManager.AppSettings["smtpServer"];
                SmtpServer.Port = port;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(fromAddress, password);
                SmtpServer.EnableSsl = enablessl;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool SendLeaveApplicationMail(string mailTo, Users user,  string subjectTitle,string reqCode)
        {
            try
            {

                string fromAddress = ConfigurationManager.AppSettings["smtpUser"];
                string password = ConfigurationManager.AppSettings["smtpPass"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
                bool enablessl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromAddress);
                mail.To.Add(mailTo);
                mail.Subject = subjectTitle;
                mail.Body = PopulateBody(user.Managedby, user.Empname, reqCode);
                mail.IsBodyHtml = true;

                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Host = ConfigurationManager.AppSettings["smtpServer"];
                SmtpServer.Port = port;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(fromAddress,password);
                SmtpServer.EnableSsl = enablessl;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion SMTP settings

        #region Email Body

        private static string PopulateBody(string managername, string Empname, string requestcode)
        {
            string body = string.Empty;
            body += "Hi " + managername + ", <br />";
            body += "You have recieved a leave application from " + Empname + ". <br />";
            body += "Please go through the request code : " + requestcode;

            return body;
        }


        private static string PopulateEmpRespFromMgr(string managername, string Empname, string HrName, string reqcode)
        {
            string body = string.Empty;
            body += "Hi " + Empname + ", <br />";
            body += "Your request (" + reqcode + ") has been approved by " + managername + " and forwarded to <br />";
            body += HrName + " for approval < br />";
            return body;
        }

        private static string PopulateEmpRespFromHr(string Empname, string HrName, string reqcode)
        {
            string body = string.Empty;
            body += "Hi " + Empname + ", <br />";
            body += "Your request (" + reqcode + ") has been approved by " + HrName + " <br />";
            return body;
        }

        private static string PopulateHrRespFromMgr(string hrname, string MgrName, string reqcode)
        {
            string body = string.Empty;
            body += "Hi " + hrname + ", <br />";
            body += "A new request (" + reqcode + ") has been approved and forwarded by " + MgrName + ". <br />";
            return body;
        }

        private static string PopulateBody(string emailaddress, string password)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/ForgotPassword.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{emailaddress}", emailaddress);
            body = body.Replace("{password}", password.TrimEnd(' '));
            return body;
        }

        #endregion Email Body

        #region Create Salt Password

        public static string GetRandomPasswordSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[32];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        #endregion Create Salt Password

        #region Create Hash Password

        public static string GetPasswordHash(string password, string salt)
        {
            string saltAndPassword = String.Concat(password, salt);
            MD5 algorithm = MD5.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            string mdstring = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                mdstring += data[i].ToString("x2").ToUpperInvariant();
            }
            return mdstring;
        }

        #endregion Create Hash Password

        #region GetRandom Password

        public static string GetRandomString(int length)
        {
            string randomData = string.Empty;
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                randomData += (char)r.Next(65, 90);
            }
            return randomData;
        }

        public static string GetRandomPassword()
        {
            Random r = new Random();
            string newPassword = System.Web.Security.Membership.GeneratePassword(7, 0);
            newPassword = Regex.Replace(newPassword, @"^([a-zA-Z+]+[0-9+]+[&@!#+]+)$", m => ((char)r.Next(65, 90)).ToString()) + 0;

            return newPassword;
        }

        #endregion GetRandom Password

        public static long MakeLong(int left, int right)
        {
            //implicit conversion of left to a long
            long res = left;

            //shift the bits creating an empty space on the right
            // ex: 0x0000CFFF becomes 0xCFFF0000
            res = (res << 32);

            //combine the bits on the right with the previous value
            // ex: 0xCFFF0000 | 0x0000ABCD becomes 0xCFFFABCD
            res = res | (long)(uint)right; //uint first to prevent loss of signed bit

            //return the combined result
            return res;
        }

        public static string GuidString()
        {
            return Guid.NewGuid().ToString("N");
        }

        public enum PlanningType
        {
            dropbottom = 100105,
            bulkwaste = 100106,
        }
    }

}