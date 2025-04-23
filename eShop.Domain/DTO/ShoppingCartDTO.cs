using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShop.Domain.DomainModels;

namespace eShop.Domain.DTO
{
    public class ShoppingCartDTO
    {
        public int Sum { get; set; }
        public List<ProductInShoppingCart>? ProductsInShoppingCart { get; set; }
    }
}
