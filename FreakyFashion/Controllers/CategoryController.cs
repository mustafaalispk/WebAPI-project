using System.Diagnostics;
using System.Linq;
using HemTentan.Data;
using HemTentan.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HemTentan.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;

        private readonly ApplicationDbContext _context;
       
        public CategoryController(ApplicationDbContext context, ILogger<CategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }
           

        public IActionResult Privacy()
        {
            return View();
        }
       
        public IActionResult Details(int id)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel { Category = _context.Category
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.Id == id)
                };

            return View(categoryViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
