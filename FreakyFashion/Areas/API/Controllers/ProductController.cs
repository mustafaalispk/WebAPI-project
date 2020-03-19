using HemTentan.Data;
using HemTentan.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HemTentan.Areas.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    //Nån sätt behöver vi komma åt data i databasen och det gör vi att skapa upp en dependencies injenction
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProductController(ApplicationDbContext context)
        {
            this.context = context;
        }
        //GET /api/product
        [HttpGet]
        public IEnumerable<ProductDto> GetAll()
        {
            var products = context.Product
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Category)
                .ToList();

            var projectedProducts = products.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ArticleNumber = x.ArticleNumber,
                Price = x.Price,
                ImageUrl = x.ImageUrl,
                Categories = x.ProductCategories.Select(y => new CategoryDto
                {
                    Id = y.Category.Id,
                    Name = y.Category.Name,
                    ImageUrl = y.Category.ImageUrl
                }).ToList()
            });

            return projectedProducts;

        }

        // GET /api/product/{id}
        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = context.Product.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound(); // 404 Not Found

            }
            return product;
        }

        //POST /api/product/
        [Authorize(Policy = "IsAdministrator")]
        [HttpPost]
        public ActionResult<Product> Create(Product product)
        {
            context.Product.Add(product);
            context.SaveChanges();

            //return Created($"/api/Product/{product.Id}", product); // Location: /api/Product/1

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // DELETE /api/product/{id}
        [Authorize(Policy = "IsAdministrator")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var product = context.Product.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound(); // 404 Not Found.
            }

            context.Product.Remove(product);

            context.SaveChanges(); // Save the result in the database.

            return NoContent(); // 204 No Content
        }

        // PUT /api/product/{id}
        [Authorize(Policy = "IsAdministrator")]
        [HttpPut ("{id}")]
        public ActionResult Update(int id, Product product)
        {
            // OM  id är inte samma som product.Id då retunerar den  BadRequest() 400 ---> tillbaka
            // like id ---> 10 har inte hittat i databasen ---> product.Id
            if (id != product.Id)
            {
                return BadRequest(); // 400 Bad Request
            }

            // It saves all the changes of the properties in the database against this id number.
            context.Entry(product).State = EntityState.Modified;

            context.SaveChanges();

            return NoContent(); // 204 No Content
        }
        
    }
}
