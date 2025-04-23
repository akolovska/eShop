using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShop.Domain.DomainModels;
using eShop.Domain.DTO;

namespace eShop.Service.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product? GetById(Guid id);
        Product Insert(Product product);
        Product Update(Product product);
        Product DeleteById(Guid id);
        AddToShoppingCartDTO GetSelectedShoppingCartProduct(Guid id);
        void AddProductToShoppingCart(Guid id, Guid userId, int quantity);
    }
}
