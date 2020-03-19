using HemTentan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemTentan.Models
{
    public class CartViewModel
    {
        public Dictionary<Product, int> CardValues { get; set; } = new Dictionary<Product, int>();
        public Decimal Total { get; set; }

    }
}
