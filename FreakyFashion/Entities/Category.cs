using System.Collections.Generic;

namespace HemTentan.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }           
        public string ImageUrl { get; set; }

        public Category()
        {

        }

        public List<ProductCategory> ProductCategories { get; protected set; } = new List<ProductCategory>();
        public Category(int id, string name, string imageUrl)
        {
            Id = id;
            Name = name;
            ImageUrl = imageUrl;
        }
        public Category(string name, string imageUrl)
        {          
            Name = name;
            ImageUrl = imageUrl;
        }
    }
}
