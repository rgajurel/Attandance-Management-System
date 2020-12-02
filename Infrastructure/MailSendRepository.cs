using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Net.Mail;
using System.Net;

namespace Infrastructure
{
    public class MailSendRepository : IMailSendRepository
    {
        public bool SendMail(string to, string MailSubject, string MailBody)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(to);
                mail.From = new MailAddress("");
                mail.Subject = MailSubject;                
                mail.Body = MailBody;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; ;
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("username", "password");
                smtp.EnableSsl = true;
                //smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                //smtp.Port = 587;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new System.Net.NetworkCredential
                //("username", "password");

                ////Or your Smtp Email ID and Password
                //smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
    }
}
