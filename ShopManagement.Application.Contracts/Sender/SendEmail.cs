using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Application.Contracts.Sender
{
    public class SendEmail
    {
        public static void Send(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("sheikhimohammadali571@gmail.com", "فروشگاه");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("sheikhimohammadali571@gmail.com", "myxqolgdjlathpsu");
            SmtpServer.EnableSsl = true; // only for port 465
            SmtpServer.Send(mail);

        }
    }
}
