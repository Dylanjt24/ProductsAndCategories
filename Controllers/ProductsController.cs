using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsAndCategories.Models;

namespace ProductsAndCategories.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;

          private ProductsAndCategoriesContext db;
        public ProductsController(ProductsAndCategoriesContext context)
        {
            db = context;
        }

        [HttpGet("/products")]
        public IActionResult Index()
        {
            ViewBag.AllProducts = db.Products.ToList();
            return View();
        }

        [HttpPost("/products/create")]
        public IActionResult Create(Product newProduct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AllProducts = db.Products.ToList();
                return View("Index");
            }
            
            db.Products.Add(newProduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/products/{productId:int}")]
        public IActionResult Show(int productId)
        {
            ViewBag.ProductWithCategories = db.Products
            .Include(p => p.Categories)
            .ThenInclude(c => c.Category)
            .FirstOrDefault(p => p.ProductId == productId);

            if (ViewBag.ProductWithCategories == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.categoriesWithoutProduct = db.Categories
            .Include(c => c.Products)
            .Where(c => c.Products
            .Any(p => p.ProductId == productId) == false)
            .ToList();

            return View();
        }

        [HttpPost("/products/{productId:int}/add-category")]
        public IActionResult AddCategory(ProductInCategory newProductCategory, int productId)
        {
            db.ProductsInCategories.Add(newProductCategory);
            db.SaveChanges();
            return RedirectToAction("Show", new {productId = productId});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
