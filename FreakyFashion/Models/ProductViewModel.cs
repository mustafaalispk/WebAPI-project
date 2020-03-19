using HemTentan.Entities;
using System.Collections.Generic;

namespace HemTentan.Models
{
    public class ProductViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductCategory> RecomendedProducts { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }

    }
}
