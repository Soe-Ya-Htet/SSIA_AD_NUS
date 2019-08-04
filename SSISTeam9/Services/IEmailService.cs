using System;

namespace SSISTeam9.Services
{
    public interface IEmailService
    {
        void SendEmail(String receiverEmailAddress);
    }
}
