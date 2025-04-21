using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShop.Domain.Identity;

namespace eShop.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string? OwnerId { get; set; }
        public eShopApplicationUser? Owner { get; set; }

    }
}
