using HemTentan.Data;
using HemTentan.Entities;
using HemTentan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemTentan.Controllers
{
    public class CartController : Controller
    {

        private readonly ILogger<CartController> _logger;

        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public CartViewModel cartViewModel = new CartViewModel();
        public IActionResult Index()
        {
            Product product = _context.Product
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Category)
                //Id == 7 är Produkt Id i Produkt tabelen i databasen
                // 2 betyder att den ska hämta 2 produkter från produkt tabelen i databasen
                .FirstOrDefault(x => x.Id == 7);
            cartViewModel.CardValues.Add(product, 2);
            foreach (var cardValues in cartViewModel.CardValues)
            {
                cartViewModel.Total += (cardValues.Key.Price * cardValues.Value);
            }
            return View(cartViewModel);
        }
       

    }
}
