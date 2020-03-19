using HemTentan.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HemTentan.ViewComponents
{
    // MenuViewComponent ävrar från ViewComponents under RazorPagesToMVC.
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public MenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string menuName)
        {
            var menu = await _context.Menu
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Name == menuName);
            return View(menu);
        }
    }
}
