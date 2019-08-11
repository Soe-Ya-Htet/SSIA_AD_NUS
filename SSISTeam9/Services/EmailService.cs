﻿using SSISTeam9.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SSISTeam9.Services
{
    public enum EmailTrigger
    {
        ON_REQUISITION_MAIL,
        ON_COLLECTION_POINT_CHANGE,
        ON_LOW_STOCK,
        ON_ASSIGNED_AS_DEPT_REP
    }
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
        private void SendEmail(string receiverEmailAddress, string subject, string body)
        {

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

        public void SendMail(EmailNotification notice, EmailTrigger trigger)
        {
            PrepareSubjectAndBody(notice, trigger);
            if(!string.IsNullOrEmpty(notice.ReceiverMailAddress) && !string.IsNullOrEmpty(notice.Subject) && !string.IsNullOrEmpty(notice.Body))
                SendEmail(notice.ReceiverMailAddress, notice.Subject, notice.Body);
        }

        private void PrepareSubjectAndBody(EmailNotification notice, EmailTrigger trigger)
        {
            switch(trigger)
            {
                case EmailTrigger.ON_ASSIGNED_AS_DEPT_REP:
                    PrepareAssignedAsDeptRepMailContent(notice);
                    break;

                case EmailTrigger.ON_COLLECTION_POINT_CHANGE:
                    PrepareCollectionPointChangeMailContent(notice);
                    break;

                case EmailTrigger.ON_LOW_STOCK:
                    PrepareLowStockMailContent(notice);
                    break;

                case EmailTrigger.ON_REQUISITION_MAIL:
                    PrepareRequisitionMailContent(notice);
                    break;
            }
        }

        private void PrepareAssignedAsDeptRepMailContent(EmailNotification notice)
        {
            notice.Subject = "Email on being assigned as a department representative";
            StringBuilder builder = new StringBuilder("Dear Sir / Mdm,");
            builder.Append("<br/><br/> This is email is to confirm the assignment of <strong>")
                .Append(notice.Dept.Representative.EmpName)
                .Append(" as the ").Append(notice.Dept.DeptName).Append(" Representative for the time period from ")
                .Append(notice.From.ToString("dd/MM/yyyy")).Append(" to ").Append(notice.To.ToString("dd/MM/yyyy")).Append(".");
            notice.Body = builder.ToString();
        }

        private void PrepareRequisitionMailContent(EmailNotification notice)
        {
            notice.Subject = "Requisition Email";
            StringBuilder builder = new StringBuilder("Dear Sir / Mdm,");
            builder.Append("<br/><br/> You have a new requisition order.");
            notice.Body = builder.ToString();
        }

        private void PrepareLowStockMailContent(EmailNotification notice)
        {
            notice.Subject = "Low stock";
            StringBuilder builder = new StringBuilder("Dear Sir / Mdm,");
            builder.Append("<br/><br/> Below items in the inventory are running low. Hurry up and start ordering before your inventory runs empty! <br/><br/>");
            foreach(Inventory inventory in notice.Items) {
                builder.Append(inventory.Description).AppendLine();
            }
            notice.Body = builder.ToString();
        }

        private void PrepareCollectionPointChangeMailContent(EmailNotification notice)
        {
            notice.Subject = "Change of collection point";
            StringBuilder builder = new StringBuilder("Dear Sir / Mdm,");
            builder.Append("<br/><br/> Please be notified that there is a change in collection point for <strong>")
                .Append(notice.Dept.DeptName)
                .Append("</strong> Department to Venue : <strong>")
                .Append(notice.Dept.CollectionPoint.Name)
                .Append("</strong>.");
            notice.Body = builder.ToString();
        }
    }
}