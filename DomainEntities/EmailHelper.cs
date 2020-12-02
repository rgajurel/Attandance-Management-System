using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public class EmailHelper
    {
        public static bool SendEmail(EmailSenderReceiverData emailContent, Template template)
        {
            try
            {
                var sendermail = new MailAddress(emailContent.SMTPUserName);
                var receiveremail = new MailAddress(emailContent.EmailTo, "receiver");
                var password = emailContent.SMTPPassword;

                var subject = template.Subject;

                var body = template.Body;

                var smtp = new SmtpClient
                {
                    Host = emailContent.SMTPHost,
                    Port = emailContent.SMTPPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(sendermail.Address, password),

                };
                using (var message = new MailMessage(sendermail, receiveremail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
