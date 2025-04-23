using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShop.Domain.DomainModels;
using eShop.Domain.DTO;

namespace eShop.Service.Interfaces
{
    public interface IShoppingCartService
    {
        ShoppingCart? GetByUserId(Guid userId);
        ShoppingCartDTO GetByUserIdWithIncludedPrducts(Guid userId);
        void DeleteProductFromShoppingCart(Guid productInShoppingCartId);
        void OrderProducts(string? userId);
    }
}
