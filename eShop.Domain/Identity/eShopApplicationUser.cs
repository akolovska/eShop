using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShop.Domain.DomainModels;
using Microsoft.AspNetCore.Identity;

namespace eShop.Domain.Identity
{
    public class eShopApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
