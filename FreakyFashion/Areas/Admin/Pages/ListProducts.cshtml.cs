using HemTentan.Data;
using HemTentan.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemTentan.Areas.Admin.Pages
{
    public class ListProductsModel : PageModel
    {
        readonly ApplicationDbContext _context;
        public List<Product> Products { get; set; }

        public ListProductsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult OnGet([Bind(Prefix = "q")] string queryString)
        {
            
            
            Products = _context.Product
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Category)
                .ToList();

            if (queryString != null)
            {

               Products = _context.Product
                    .Include(x => x.ProductCategories)
                    .ThenInclude(x => x.Category)
                    .Where(x => x.Name.Contains(queryString))
                    .ToList();
            }

            return Page();
            
        }

    }
}
