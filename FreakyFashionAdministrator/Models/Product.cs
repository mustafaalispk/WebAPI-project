using System;
using System.Collections.Generic;
using System.Text;

namespace FreakyFashionAdministrator.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }
        public string ArticleNumber { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }        
        public Product()
        {

        }

        public List<Category> Categories { get; set; } 

        public Product(int id, string name, string description,string articleNumber, decimal price, string imageUrl)
        {
            Id = id;
            Name = name;            
            Description = description;
            ArticleNumber = articleNumber;
            Price = price;
            ImageUrl = imageUrl;
        }
        public Product(string name, string description, string articleNumber, decimal price, string imageUrl)
        {
            Name = name;           
            Description = description;
            ArticleNumber = articleNumber;
            Price = price;
            ImageUrl = imageUrl;            
        }
    }
}
