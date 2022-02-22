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
    public class CategoriesController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;

          private ProductsAndCategoriesContext db;
        public CategoriesController(ProductsAndCategoriesContext context)
        {
            db = context;
        }

        [HttpGet("/categories")]
        public IActionResult Index()
        {
            ViewBag.AllCategories = db.Categories.ToList();
            return View();
        }

        [HttpPost("/categories/create")]
        public IActionResult Create(Category newCategory)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AllCategories = db.Categories.ToList();
                return View("Index");
            }

            db.Categories.Add(newCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/categories/{categoryId:int}")]
        public IActionResult Show(int categoryId)
        {
            ViewBag.CategoryWithProducts = db.Categories
            .Include(c => c.Products)
            .ThenInclude(p => p.Product)
            .FirstOrDefault(c => c.CategoryId == categoryId);

            if (ViewBag.categoryWithProducts == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.ProductsNotInCategory = db.Products
            .Include(p => p.Categories)
            .Where(p => p.Categories
            .Any(c => c.CategoryId == categoryId) == false)
            .ToList();

            return View();
        }

        [HttpPost("/categories/{categoryId:int}/add-product")]
        public IActionResult AddProduct(ProductInCategory newCategoryProduct, int categoryId)
        {
            db.ProductsInCategories.Add(newCategoryProduct);
            db.SaveChanges();
            return RedirectToAction("Show", new {categoryId = categoryId});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
