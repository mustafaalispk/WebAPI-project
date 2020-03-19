using HemTentan.Data;
using HemTentan.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemTentan.Areas.Admin.Pages
{
    public class AddProductModel : PageModel
    {
        private readonly ILogger<AddProductModel> _logger;

        private readonly ApplicationDbContext _context;
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }

        public Product Product { get; set; }

        public AddProductModel(ApplicationDbContext context, ILogger<AddProductModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnPost(ProductDto productDto)
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
                urlSlug: urlSlug.Value);
                var productCategory = new ProductCategory
                {
                    CategoryId = productDto.CategoryId
                };
                product.ProductCategories.Add(productCategory);
                _context.Product.Add(product);
                _context.SaveChanges();
                return RedirectToPage("ListProducts");
            }
            catch
            {
                return RedirectToPage("AddProduct");
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
        public void OnGet()
        {
            
            Categories = _context.Category.ToList();

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

    }
}
