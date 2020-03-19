﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HemTentan.Entities
{
    public class ProductCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ArticleNumber { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public IList<Category> Categories { get; set; } = new List<Category>();
    }
}
