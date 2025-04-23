using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain.DomainModels
{
    public class EmailMessage : BaseEntity
    {
        public string? UserId { get; set; }
        public string? Message { get; set; }
        public string? Subject { get; set; }
        public bool? Status { get; set; }
    }
}
