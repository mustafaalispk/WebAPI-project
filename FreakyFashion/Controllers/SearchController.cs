using HemTentan.Data;
using HemTentan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HemTentan.Controllers
{
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;

        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context, ILogger<SearchController> logger)
        {
            _context = context;
            _logger = logger;
        }
        SearchViewModel searchViewModel = new SearchViewModel();
        // GET: Search
        public async Task<IActionResult> Index([Bind(Prefix = "q")] string queryString)
        {
            searchViewModel.SearchResult = _context.Product.Where(x => x.Name.Contains(queryString)).ToList();
            searchViewModel.RecomendedProducts = _context.ProductCategory
                .Include(x => x.Product)
                .Take(4)
                .ToList();
            return View(searchViewModel);
        }
        
    }
    }

