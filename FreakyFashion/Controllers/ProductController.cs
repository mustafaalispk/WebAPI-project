using System;
using System.Diagnostics;
using System.Linq;
using HemTentan.Data;
using HemTentan.Entities;
using HemTentan.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HemTentan.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        private readonly ApplicationDbContext _context;
       
        public ProductController(ApplicationDbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }
       
        // POST: Product/New
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public ActionResult New(ProductDto productDto)
        {
            try
            {
                var urlSlug = new UrlSlug(productDto.Name);
                
                // TODO: Add insert logic here
                Product product = new Product(
                productDto.Name,
                productDto.ArticleNumber,
                productDto.Description,
                productDto.Price,
                productDto.ImageUrl,
                urlSlug: urlSlug.Value );
                var productCategory = new ProductCategory
                {
                    CategoryId = productDto.CategoryId
                };
                product.ProductCategories.Add(productCategory);
                _context.Product.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(New));
            }
            catch
            {
                return RedirectToAction(nameof(New));
            }
        }    
        public class UrlSlug
        {
            public string Value { get; private set; }            
           
            public UrlSlug(string productName)
            {
               Value = productName.ToLower().Replace(' ', '-');
               
            }
        }
        public class ProductDto
        {
            public string Name { get; set; }
            public string ArticleNumber { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string ImageUrl { get; set; }
            public int CategoryId { get; set; }
        }

        public IActionResult Privacy()
        {
            return View();
        }
       
        public IActionResult Details(string urlSlug)
        {
            ProductViewModel productViewModel = new ProductViewModel { Product = _context.Product
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Category)
                .FirstOrDefault(x => x.UrlSlug == urlSlug),
            RecomendedProducts = _context.ProductCategory
           .Include(x => x.Product)
           .OrderByDescending(x => Guid.NewGuid())
           .Take(4)
           .ToList()
            };
            return View(productViewModel);          

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
