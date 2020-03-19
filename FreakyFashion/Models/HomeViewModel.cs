using HemTentan.Entities;
using System.Collections.Generic;

namespace HemTentan.Models
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
