using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain.DomainModels
{
    public class Product : BaseEntity
    {
        [Required]
        public string? ProductName { get; set; }

        [Required] 
        public string? ProductDescription { get; set; }

        [Required] 
        public string? ProductImage { get; set; }
        [Required]
        public int ProductPrice { get; set; }
        [Required]
        public int Rating { get; set; }
        public virtual ICollection<ProductInShoppingCart>? ProductInShoppingCarts { get; set; }


    }
}
