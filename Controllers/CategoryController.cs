using EcommerceProductManagement.Data;
using EcommerceProductManagement.Models;
using EcommerceProductManagement.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllCategories()
        {
            return Ok(await appDbContext.Categories.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var categoryEntity = await appDbContext.Categories
                .Where(c => c.Id == id)
                .Select(c => new Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Products = c.Products,
                }).FirstOrDefaultAsync();

            if (categoryEntity == null)
            {
                return NotFound();
            }

            return Ok(categoryEntity);
        }


        [HttpPost]
        public async Task<ActionResult<AddCategoryDTO>> AddCategory(AddCategoryDTO addCategoryDTO)
        {
            // Validate the incoming DTO
            if (addCategoryDTO == null || string.IsNullOrWhiteSpace(addCategoryDTO.Name) || addCategoryDTO.Products == null || !addCategoryDTO.Products.Any())
            {
                return BadRequest("Invalid category data. Please provide a name and a list of associated products.");
            }
            var categoryEntity = new Category()
            {
                Name = addCategoryDTO.Name,
                Description = addCategoryDTO.Description,
                Products = new List<Product>()
            };

            var notFoundProducts = new List<int>(); 

            //Fetch products if exist
            foreach (var productDTO in addCategoryDTO.Products)
            {
                var product = await appDbContext.Products.FindAsync(productDTO.Id);
                if (product != null)
                {
                    categoryEntity.Products.Add(product);
                }
                else
                {
                    notFoundProducts.Add(productDTO.Id);
                }
            }
      
            if (notFoundProducts.Any())
            {
                return NotFound(new { Message = "Some products were not found.", MissingProductIds = notFoundProducts });
            }


            appDbContext.Categories.Add(categoryEntity);
            await appDbContext.SaveChangesAsync();

            // Map existing products to ProductDTO for the response
            var productDtos = categoryEntity.Products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            }).ToList();

            // Update the DTO with the new category ID and product details
            addCategoryDTO.Id = categoryEntity.Id;
            addCategoryDTO.Products = productDtos;

            return CreatedAtAction(nameof(GetCategory), new { id = addCategoryDTO.Id }, addCategoryDTO);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateCategoryDTO>> UpdateCategory(int id, UpdateCategoryDTO updateCategoryDTO)
        {
            // Validate the incoming DTO
            if (updateCategoryDTO == null || string.IsNullOrWhiteSpace(updateCategoryDTO.Name))
            {
                return BadRequest("Invalid category data.");
            }

            // Check if the category exists
            var categoryEntity = await appDbContext.Categories
                .Include(c => c.Products) // Include related products
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoryEntity == null) 
            {
                return NotFound($"Category with ID {id} not found.");
            }
            // update changes
            categoryEntity.Name = updateCategoryDTO.Name;
            categoryEntity.Description = updateCategoryDTO.Description;

            // Clear existing products
            categoryEntity.Products.Clear();

            var notFoundProducts = new List<int>(); 

            // Retrieve existing products from db to category
            foreach (var productDTO in updateCategoryDTO.Products)
            {
                var product = await appDbContext.Products.FindAsync(productDTO.Id);
                if (product != null)
                {
                    categoryEntity.Products.Add(product);
                }
                else
                {
                    notFoundProducts.Add(productDTO.Id); // Add to not found list
                }
            }

            // If any products were not found, return a warning message
            if (notFoundProducts.Count > 0)
            {
                return NotFound(new
                {
                    Message = "Some products were not found.",
                    MissingProductIds = notFoundProducts
                });
            }

            //appDbContext.Entry(categoryEntity).State = EntityState.Modified;
            await appDbContext.SaveChangesAsync();

            // Map updated products to ProductDTO for the response
            var productDtos = categoryEntity.Products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            }).ToList();

            // Create an updated DTO to return
            var updatedCategoryDTO = new UpdateCategoryDTO
            {
                Id = categoryEntity.Id,
                Name = categoryEntity.Name,
                Description = categoryEntity.Description,
                Products = productDtos
            };

            return Ok(updatedCategoryDTO);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var categoryEntity = await appDbContext.Categories.FindAsync(id);

            if (categoryEntity == null)
            {
                return NotFound();
            }

            appDbContext.Categories.Remove(categoryEntity);
            await appDbContext.SaveChangesAsync();

            return Ok(categoryEntity);

        }


    }
}
