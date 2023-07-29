using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace ArticlProjects.Code
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials=new NetworkCredential("adel30320@gmail.com", "qaydxscluqcehuhf"),
                EnableSsl = true,
            };

          return  smtpClient.SendMailAsync("adel30320@gmail.com", email, subject, htmlMessage);
        }
    }
}
