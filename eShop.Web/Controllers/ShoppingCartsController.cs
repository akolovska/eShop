using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eShop.Domain.DomainModels;
using eShop.Service.Interfaces;
using eShop.Web.Data;
using eShop.Service.Implementations;
using System.Security.Claims;
using eShop.Domain.DTO;

namespace eShop.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService _context;

        public ShoppingCartsController(IShoppingCartService context)
        {
            _context = context;
        }

        // GET: ShoppingCarts
        public ActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ShoppingCartDTO userShoppingCart = _context.GetByUserIdWithIncludedPrducts(Guid.Parse(userId));
            return View(userShoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public IActionResult Delete(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _context.DeleteProductFromShoppingCart(Guid.Parse(userId));
            return RedirectToAction(nameof(Index));
        }
        public IActionResult OrderNow()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _context.OrderProducts(userId);
            return RedirectToAction(nameof(Index));
        }

    }
}
