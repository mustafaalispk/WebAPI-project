using HemTentan.Data;
using HemTentan.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HemTentan.Areas.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public CategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }
        //GET /api/categories

        [HttpGet]
        public IEnumerable<Category> GetAll()
        {
            var categories = context.Category.ToList();
            return categories;
        }

        // HTTPGet anropp för att hämta category data genom id.

        [HttpGet("{id}")]
        public ActionResult<Category> GetById(int id)
        {
            var category = context.Category.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound(); // 404 Not Found

            }
            return category;
        }

        //HttpPost annrop som skapar en ny category och lägger in en category.
        //POST /api/category/
        [Authorize(Policy = "IsAdministrator")]
        [HttpPost]
        public ActionResult<Category> Create(Category category)
        {
            //sparar och lägger in ------> category i databasen
            context.Category.Add(category);
            context.SaveChanges();

            //return Created($"/api/Category/{category.Id}", category); // Location: /api/Category/1

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        // /api/category/{id}/product
        [Authorize(Policy = "IsAdministrator")]
        [HttpPost ("{categoryId}/product")]
        public ActionResult AddProductToCategory(int categoryId, ProductDto product)
        {
            // Här under kollar den min produckt Id och category Id i databasen om det finns inte då visar den 0.
            ProductCategory productCategory = new ProductCategory(product.Id,categoryId);
            context.ProductCategory.Add(productCategory);
            context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // DELETE /api/category/{id}
        [Authorize(Policy = "IsAdministrator")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var category = context.Category.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound(); // 404 Not Found.
            }

            context.Category.Remove(category);

            context.SaveChanges(); // Save the result in the database.

            return NoContent(); // 204 No Content
        }

        // PUT /api/category/{id}
        [Authorize(Policy = "IsAdministrator")]
        [HttpPut("{id}")]
        public ActionResult Update(int id, Category category)
        {
            // Här jämför jag två typer som inte går att jämpöra 
            // en typ som är Product och en är Int32, som bilen är inte samma som hjulet.
            if (id != category.Id)
            {
                return BadRequest(); // 400 Bad Request
            }

            // It saves all the changes of the properties in the database against this id number.
            context.Entry(category).State = EntityState.Modified;

            context.SaveChanges();

            return NoContent(); // 204 No Content
        }

    }
}
