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
using Microsoft.AspNetCore.Authorization;
using eShop.Domain.DTO;

namespace eShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _context;

        public ProductsController(IProductService context)
        {
            _context = context;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_context.GetAll());
        }

        // GET: Products/Details/5
        public IActionResult Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProductName,ProductDescription,ProductImage,ProductPrice,Rating,Id")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Insert(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("ProductName,ProductDescription,ProductImage,ProductPrice,Rating,Id")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            _context.Update(product);
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var product = _context.GetById(id);
            if (product != null)
            {
                _context.DeleteById(product.Id);
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult AddProductToCart(Guid id)
        {
            AddToShoppingCartDTO cartDTO = _context.GetSelectedShoppingCartProduct(id);
            return View(cartDTO);
        }

        [HttpPost]
        public IActionResult AddProductToCart(AddToShoppingCartDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _context.AddProductToShoppingCart(model.ProductId, Guid.Parse(userId), model.Quantity);
            return RedirectToAction(nameof(Index));
        }
    }
}
