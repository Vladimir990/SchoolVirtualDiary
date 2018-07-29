using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using virtual_diary.Models;
using virtual_diary.Repositories;

namespace virtual_diary.Services
{
    public class EmailService: IEmailService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public void SendMail(StudentTeacherSubject sts, int value)
        {
            string subject = "Student grade";
            string body = string.Format(@"Postovani gospodine/gospodjo {0}, obavestavamo Vas da je Vase dete dobilo ocenu {1} iz predmete {2} koji predaje nastavnik {3} {4}", sts.Student.Parent.LastName, value , sts.TeacherSubject.Subject.Name, sts.TeacherSubject.Teacher.FirstName, sts.TeacherSubject.Teacher.LastName);
            string FromMail = ConfigurationManager.AppSettings["from"];
            //string emailTo = "";
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"]);
            mail.From = new MailAddress(FromMail);
            mail.To.Add(string.Format("{0}", sts.Student.Parent.Email));
            mail.Subject = subject;
            mail.Body = body;
            SmtpServer.Port = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["from"], ConfigurationManager.AppSettings["password"]);
            SmtpServer.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["smtpSsl"]);
            SmtpServer.Send(mail);
            logger.Info("Email is sent to address: {0}", sts.Student.Parent.Email);
        }
    }
}