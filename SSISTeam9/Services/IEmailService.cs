using SSISTeam9.Models;
using System;

namespace SSISTeam9.Services
{
    public interface IEmailService
    {
        void SendMail(EmailNotification notice, EmailTrigger trigger);
    }
}
