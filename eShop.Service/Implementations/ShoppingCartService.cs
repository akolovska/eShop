using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using eShop.Domain.DomainModels;
using eShop.Domain.DTO;
using eShop.Repository.Interfaces;
using eShop.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop.Service.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<ProductInShoppingCart> _productInShoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<ProductInOrder> _productsInOrderRepository;
        private readonly IEmailService _emailService;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<ProductInShoppingCart> productInShoppingCartRepository, IRepository<Order> orderRepository, IRepository<ProductInOrder> productsInOrderRepository, IEmailService emailService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
            _orderRepository = orderRepository;
            _productsInOrderRepository = productsInOrderRepository;
            _emailService = emailService;
        }

        public ShoppingCart? GetByUserId(Guid userId)
        {
            return _shoppingCartRepository.Get(selector: x => x,
                                                       predicate: x => x.OwnerId.Equals(userId.ToString()));
        }

        public void DeleteProductFromShoppingCart(Guid productInShoppingCartId)
        {
            var prodictInShoppingCart = _productInShoppingCartRepository.Get(selector: x => x,
                predicate: x => x.Id.Equals(productInShoppingCartId));

            if (prodictInShoppingCart == null)
            {
                throw new Exception("Product in shopping cart not found");
            }

            _productInShoppingCartRepository.Delete(prodictInShoppingCart);
        }

        public ShoppingCartDTO GetByUserIdWithIncludedPrducts(Guid userId)
        {
            var userCart = _shoppingCartRepository.Get(selector: x => x,
                                               predicate: x => x.OwnerId.Equals(userId.ToString()),
                                               include: x => x.Include(z => z.ProductsInShoppingCarts)
                                               .ThenInclude(m => m.Product));

            var allProducts = userCart.ProductsInShoppingCarts.ToList();

            var allProductPrices = allProducts.Select(z => new
            {
                ProductPrice = z.Product.ProductPrice,
                Quantity = z.Quantity
            }).ToList();

            double totalPrice = 0.0;

            foreach (var item in allProductPrices)
            {
                totalPrice += item.Quantity * item.ProductPrice;
            }

            ShoppingCartDTO model = new ShoppingCartDTO
            {
                ProductsInShoppingCart = allProducts,
                Sum = totalPrice
            };

            return model;
        }
        public void OrderProducts(string userId)
        {
            var shoppingCart = _shoppingCartRepository.Get(selector: x => x,
                    predicate: x => x.OwnerId == userId,
                    include: x => x.Include(y => y.ProductsInShoppingCarts)
                    .ThenInclude(z => z.Product)
                    .Include(y => y.Owner));

            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                Owner = shoppingCart.Owner,
                OwnerId = userId
            };
            _orderRepository.Insert(newOrder);

            EmailMessage message = new EmailMessage();
            message.Subject = "Successfull order";
            message.UserId = shoppingCart.Owner.Email;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Your order is completed. The order conatins: ");

            var productsInOrder = shoppingCart.ProductsInShoppingCarts.Select(z => new ProductInOrder
            {
                OrderedProduct = z.Product,
                ProductId = z.ProductId,
                Order = newOrder,
                OrderId = newOrder.Id,
                Quantity = z.Quantity
            });

            var total = 0.0;

            foreach (var product in productsInOrder)
            {
                total += (product.Quantity * product.OrderedProduct.ProductPrice);
                _productsInOrderRepository.Insert(product);
                sb.AppendLine(product.OrderedProduct.ProductName + " with quantity of: " + product.Quantity + " and price of: $" + product.OrderedProduct.ProductPrice);
            }

            sb.AppendLine("Total price for your order: " + total.ToString());
            message.Message = sb.ToString();

            shoppingCart.ProductsInShoppingCarts.Clear();
            _shoppingCartRepository.Update(shoppingCart);
            _emailService.SendEmailAsync(message);


        }
    }
}
