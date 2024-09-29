using EcommerceProductManagement.Data;
using EcommerceProductManagement.Models;
using EcommerceProductManagement.Models.Entity;
using EcommerceProductManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly ProductService productService;

        public ProductController(AppDbContext appDbContext, ProductService productService)
        {
            this.appDbContext = appDbContext;
            this.productService = productService;
        }
        //endpoint specific category
        [HttpGet("category/{category.Id}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await productService.GetProductByCategory(categoryId);
            return  Ok(products);
        }

        //endpoint orders last month
        [HttpGet("orders/last-month")]
        public async Task<IActionResult> GetOrdersLastMonth()
        {
            var orders = await productService.GetOrdersLastMonth();
            return Ok(orders);
        }
        // endpoint total sales per product
        [HttpGet("sales/total")]
        public async Task<IActionResult> GetTotalSalesOfProduct()
        {
            var totalSales = await productService.GetTotalSalesOfProduct();
            return Ok(totalSales);
        }
        // endpoint top 5
        [HttpGet("sales/top5")]
        public async Task<IActionResult> GetTopFiveProduct()
        {
            var topFive = await productService.GetTopFiveProduct();
            return Ok(topFive);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return Ok(await appDbContext.Products.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var productEntity = await appDbContext.Products
                .Where(p => p.Id == id)
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    Categories = p.Categories
                }).FirstOrDefaultAsync();

            if (productEntity == null)
            {
                return NotFound();
            }

            return Ok(productEntity);
        }


        [HttpPost]
        public async Task<ActionResult<AddProductDTO>> AddProduct(AddProductDTO addProductDTO)
        {
            // Validate the incoming DTO
            if (addProductDTO == null || string.IsNullOrWhiteSpace(addProductDTO.Name) || addProductDTO.Categories == null || !addProductDTO.Categories.Any())
            {
                return BadRequest("Invalid product data. Please provide a name and associated categories.");
            }

      
            var productEntity = new Product()
            {
                Name = addProductDTO.Name,
                Description = addProductDTO.Description,
                Price = addProductDTO.Price,
                StockQuantity = addProductDTO.StockQuantity,
                Categories = new List<Category>()
            };

     
            var notFoundCategories = new List<int>();

            // Add valid categories to the product
            foreach (var categoryDTO in addProductDTO.Categories)
            {
                var category = await appDbContext.Categories.FindAsync(categoryDTO.Id);
                if (category != null)
                {
                    productEntity.Categories.Add(category);
                }
                else
                {
                    notFoundCategories.Add(categoryDTO.Id); // Track not found category IDs
                }
            }

     
            if (notFoundCategories.Any())
            {
                return NotFound(new { Message = "Some categories were not found.", MissingCategoryIds = notFoundCategories });
            }

            // Add the new product to the database
            appDbContext.Products.Add(productEntity);
            await appDbContext.SaveChangesAsync();

      

         
            var categoryDtos = productEntity.Categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();

            // Update the DTO with the new product ID
            addProductDTO.Id = productEntity.Id;
            addProductDTO.Categories = categoryDtos;

            // Return a Created response with the new product and its associated categories
            return CreatedAtAction(nameof(GetProduct), new { id = addProductDTO.Id }, addProductDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateProductDTO>> UpdateProduct(int id,UpdateProductDTO updateProductDTO)
        {
            // Validate the incoming DTO
            if (updateProductDTO == null || string.IsNullOrWhiteSpace(updateProductDTO.Name))
            {
                return BadRequest("Invalid product data. Please provide a name and other necessary details.");
            }

            // Check if the product exists
            var productEntity = await appDbContext.Products
                .Include(p => p.Categories) 
                .FirstOrDefaultAsync(p => p.Id == id);

            if (productEntity == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            // Update the product details
            productEntity.Name = updateProductDTO.Name;
            productEntity.Description = updateProductDTO.Description;
            productEntity.Price = updateProductDTO.Price;
            productEntity.StockQuantity = updateProductDTO.StockQuantity;

            // Clear existing categories
            productEntity.Categories.Clear();

            var notFoundCategories = new List<int>(); 

            // Retrieve and assign new categories
            foreach (var categoryDTO in updateProductDTO.Categories)
            {
                var category = await appDbContext.Categories.FindAsync(categoryDTO.Id);
                if (category != null)
                {
                    productEntity.Categories.Add(category);
                }
                else
                {
                    notFoundCategories.Add(categoryDTO.Id); 
                }
            }

            // If any categories were not found, return a warning message
            if (notFoundCategories.Count > 0)
            {
                return NotFound(new
                {
                    Message = "Some categories were not found.",
                    MissingCategoryIds = notFoundCategories
                });
            }

          
            await appDbContext.SaveChangesAsync();

            // Map updated categories to CategoryDTO for response
            var categoryDtos = productEntity.Categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            }).ToList();

            // Prepare updated response DTO
            var updatedProductDTO = new UpdateProductDTO
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Description = productEntity.Description,
                Price = productEntity.Price,
                StockQuantity = productEntity.StockQuantity,
                Categories = categoryDtos
            };

            return Ok(updatedProductDTO);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var productEntity = await appDbContext.Products.FindAsync(id);

            if (productEntity == null)
            {
                return NotFound();
            }

            appDbContext.Products.Remove(productEntity);
            await appDbContext.SaveChangesAsync();

            return Ok(productEntity);

        }


    }
}
