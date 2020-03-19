using HemTentan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemTentan.Models
{
    public class SearchViewModel
    {
        
        public List<Product> SearchResult { get; set; }
        public List<ProductCategory> RecomendedProducts { get; set; }
       
    }
}
