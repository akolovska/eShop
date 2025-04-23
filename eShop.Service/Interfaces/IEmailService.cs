using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using eShop.Domain.DomainModels;

namespace eShop.Service.Interfaces
{
    public interface IEmailService
    {
        void SendEmailAsync(EmailMessage allMails);
    }
}
