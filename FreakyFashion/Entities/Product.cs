using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HemTentan.Entities
{
    public class Product
    {      
        public int Id { get; set; }
        public string Name { get; set; }
        public string ArticleNumber { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string UrlSlug { get; set; }

        public Product()
        {

        }

        public List<ProductCategory> ProductCategories { get; protected set; } = new List<ProductCategory>();

        public Product(int id, string name, string articleNumber, string description, decimal price, string imageUrl, string urlSlug)
        {
            Id = id;
            Name = name;
            ArticleNumber = articleNumber;
            Description = description;
            Price = price;            
            ImageUrl = imageUrl;
            UrlSlug = urlSlug;
        }
        public Product(string name, string articleNumber, string description, decimal price, string imageUrl, string urlSlug)
        {
            Name = name;
            ArticleNumber = articleNumber;
            Description = description;
            Price = price;
            ImageUrl = imageUrl;
            UrlSlug = urlSlug;
        }
    }
}
