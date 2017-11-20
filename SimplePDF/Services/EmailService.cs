using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace SimplePDF.Services
{
    public class EmailService 
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public byte[] FileStream { get; set; }
        public string FileName { get; set; }
        public SendGridMessage myEmail { get; set; }

        /// <summary>
        /// To create the email message object and assign the properties of the message to it
        /// </summary>
        private void configSendGridMessage()
        {
            myEmail = new SendGridMessage();
            myEmail.Subject = Subject;
            myEmail.HtmlContent = Message;
            myEmail.From = new EmailAddress(FromEmail, FromName);
            myEmail.AddTo(ToEmail);
            myEmail.AddAttachment(FileName+".pdf", Convert.ToBase64String(FileStream));
        }

        /// <summary>
        /// to create sendgrid client object and call the send email function asyncrounously
        /// </summary>
        /// <returns></returns>
        public async Task sendEmailAsync()
        {
            configSendGridMessage();
            SendGridClient emailClient = new SendGridClient(ConfigurationManager.AppSettings["APIKey"]);
            await emailClient.SendEmailAsync(myEmail);
        }

      

    }
}