using SSISTeam9.Models;
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
        ON_ASSIGNED_AS_DEPT_REP,
        ON_ALTERNATIVE_SUPPLIER,
        ON_PENDING_ADJVOUCHER
        ON_DELEGATED_AS_DEPT_HEAD,
        ON_ALTERNATIVE_SUPPLIER
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
            try
            {
                PrepareSubjectAndBody(notice, trigger);
                if (!string.IsNullOrEmpty(notice.ReceiverMailAddress) && !string.IsNullOrEmpty(notice.Subject) && !string.IsNullOrEmpty(notice.Body))
                    SendEmail(notice.ReceiverMailAddress, notice.Subject, notice.Body);
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
        }

        private void PrepareSubjectAndBody(EmailNotification notice, EmailTrigger trigger)
        {
            switch(trigger)
            {
                case EmailTrigger.ON_ASSIGNED_AS_DEPT_REP:
                    PrepareAssignedAsDeptRepMailContent(notice);
                    break;

                case EmailTrigger.ON_DELEGATED_AS_DEPT_HEAD:
                    PrepareAssignedAsDeptHeadMailContent(notice);
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

                case EmailTrigger.ON_ALTERNATIVE_SUPPLIER:
                    PrepareNotificationEmailToPurchasingDepartment(notice);
                    break;
                case EmailTrigger.ON_PENDING_ADJVOUCHER:
                    PrepareNotificationEmailToPurchasingDepartment(notice);
                    break;
            }
        }

        private void PrepareAssignedAsDeptRepMailContent(EmailNotification notice)
        {
            notice.Subject = "Email on being assigned as a department representative";
            StringBuilder builder = new StringBuilder("Dear Sir / Mdm,");
            builder.Append("<br/><br/> This email is to notify that you are being assigned as Representative of your department.");
            notice.Body = builder.ToString();
        }

        private void PrepareAssignedAsDeptHeadMailContent(EmailNotification notice)
        {
            notice.Subject = "Email on being assigned as a department head";
            StringBuilder builder = new StringBuilder("Dear Sir / Mdm,");
            builder.Append("<br/><br/> This is email is to notify that you are assigned as Head for your department")
                .Append(" from ").Append(notice.From.ToString("dd/MM/yyyy")).Append(" to ").Append(notice.To.ToString("dd/MM/yyyy")).Append(".");
            notice.Body = builder.ToString();
        }

        private void PrepareRequisitionMailContent(EmailNotification notice)
        {
            notice.Subject = "Requisition Email";
            StringBuilder builder = new StringBuilder("Dear Sir / Mdm,");
            builder.Append("<br/><br/> You have a new requisition order waiting for approval.");
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

        private void PrepareNotificationEmailToPurchasingDepartment(EmailNotification notice)
        {
            notice.Subject = "Purchase from Alternative Supplier";
            StringBuilder builder = new StringBuilder("Dear Sir / Mdm,");
            builder.Append("<br/><br/> Please be informed that we have ordered from an alternative supplier as the main supplier could not fulfill required order. <br/>")
               .Append($"Order Number : {notice.Order.OrderNumber}<br/>")
               .Append($"Order Date : {notice.Order.OrderDate.ToString("dd MMM yyyy")}<br/>")
               .Append($"Alternative Supplier Name : {notice.Order.Supplier.Name}<br/>");

            foreach (var item in notice.Order.ItemDetails)
            {
                builder.Append($"Item Code: {item.Item.ItemCode}, Description: {item.Item.Description}, Quantity: {item.Quantity} <br/>");
            }
            notice.Body = builder.ToString();
        }


        private void PrepareNotificationEmailToStoreAuthorise(EmailNotification notice)
        {
            notice.Subject = "Pending Authorisation for Adjustment Voucher";
            StringBuilder builder = new StringBuilder("Dear Sir / Mdm,");
            builder.Append("<br/><br/> You have a new adjustment voucher pending for authorisation.");
            notice.Body = builder.ToString();
        }
    }
}