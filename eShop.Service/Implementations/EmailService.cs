using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using eShop.Domain.DomainModels;
using eShop.Service.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace eShop.Service.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _mailSettings;
  

        public EmailService(IOptions<EmailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public void SendEmailAsync(EmailMessage allMails)
        {
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress("EShop Application", "zorica.karapancheva@finki.ukim.mk"),
                Subject = allMails.Subject
            };

            emailMessage.From.Add(new MailboxAddress("EShop Application", "zorica.karapancheva@finki.ukim.mk"));

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = allMails.Message
            };

            emailMessage.To.Add(new MailboxAddress(allMails.UserId, allMails.UserId));

            try
            {
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    var socketOptions = SecureSocketOptions.Auto;

                    smtp.Connect(_mailSettings.SmtpServer, 587, socketOptions);

                    if (!string.IsNullOrEmpty(_mailSettings.SmtpUserName))
                    {
                        smtp.Authenticate(_mailSettings.SmtpUserName, _mailSettings.SmtpPassword);
                    }
                    smtp.Send(emailMessage);


                    smtp.Disconnect(true);

                }
            }
            catch (SmtpException ex)
            {
                throw ex;
            }
        }
    }
}
