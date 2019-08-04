using System;
using System.Net;
using System.Net.Mail;

namespace SSISTeam9.Services
{
    public class EmailService : IEmailService
    {
        private MailAddress senderEmail;
        private SmtpClient smtp;
        public EmailService()
        {
            senderEmail = new MailAddress("team9rockz@gmail.com", "Team9");
            var password = "mylovelyteam";
            smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
        }
        public void SendEmail(String receiverEmailAddress)
        {
            var subject = "Subject";
            var body = "Body";

            var receiverEmail = new MailAddress(receiverEmailAddress);
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(mess);
            }
        }
    }
}