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
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public OrderController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            return Ok(await appDbContext.Orders.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var orderEntity = await appDbContext.Orders
                .Where(o => o.Id == id)
                .Select(o => new Order
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName,
                    OrderDate = o.OrderDate,
                    OrderItems = o.OrderItems,
                }).FirstOrDefaultAsync();

            if (orderEntity == null)
            {
                return NotFound();
            }

            return Ok(orderEntity);
        }


        [HttpPost]
        public async Task<ActionResult<AddOrderDTO>> AddOrder(AddOrderDTO addOrderDTO)
        {

            // Validate the incoming DTO
            if (addOrderDTO == null || string.IsNullOrWhiteSpace(addOrderDTO.CustomerName) || addOrderDTO.OrderItems == null || !addOrderDTO.OrderItems.Any())
            {
                return BadRequest("Invalid order data. Please provide a customer name and a list of order items.");
            }
            var orderEntity = new Order()
            {
                CustomerName = addOrderDTO.CustomerName,
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>() 
            };

            var notFoundProducts = new List<int>(); 

            foreach (var itemDTO in addOrderDTO.OrderItems)
            {
                // Fetch the product by ID
                var product = await appDbContext.Products.FindAsync(itemDTO.ProductId);
                if (product != null)
                {
                   
                    if (itemDTO.Quantity > product.StockQuantity)
                    {
                        return BadRequest($"Insufficient stock for product ID {itemDTO.ProductId}. Available: {product.StockQuantity}");
                    }

                    // Create the order item
                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = itemDTO.Quantity,
                        UnitPrice = product.Price 
                    };

                    orderEntity.OrderItems.Add(orderItem); 
                    product.StockQuantity -= itemDTO.Quantity;

                }
                else
                {
                    notFoundProducts.Add(itemDTO.ProductId); 
                }
            }

            // Check if any products were not found
            if (notFoundProducts.Any())
            {
                return NotFound(new { Message = "Some products were not found.", MissingProductIds = notFoundProducts });
            }
            //Add data
            appDbContext.Orders.Add(orderEntity);
            await appDbContext.SaveChangesAsync();

            // Prepare the response DTO
            var orderItemDtos = orderEntity.OrderItems.Select(oi => new OrderItemDTO
            {
                Id = oi.Id, 
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            }).ToList();

            addOrderDTO.Id = orderEntity.Id;
            addOrderDTO.OrderItems = orderItemDtos;

            return CreatedAtAction(nameof(GetOrder), new { id = addOrderDTO.Id }, addOrderDTO);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateOrderDTO>> UpdateOrder(int id, UpdateOrderDTO updateOrderDTO)
        {
            // Validate the incoming DTO
            if (updateOrderDTO == null || updateOrderDTO.OrderItems == null || !updateOrderDTO.OrderItems.Any())
            {
                return BadRequest("Invalid order data. Please provide a list of order items.");
            }

            var existingOrder = await appDbContext.Orders.Include(o => o.OrderItems)
                                                       .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null) 
            {
                return NotFound($"Order with ID {id} not found.");
            }

            // Update the customer name if provided
            if (!string.IsNullOrWhiteSpace(updateOrderDTO.CustomerName))
            {
                existingOrder.CustomerName = updateOrderDTO.CustomerName;
            }

            
            var notFoundProducts = new List<int>();

          
            foreach (var itemDTO in updateOrderDTO.OrderItems)
            {
                // Check if the item already exists in the order
                var existingOrderItem = existingOrder.OrderItems.FirstOrDefault(oi => oi.Id == itemDTO.Id);

                if (existingOrderItem != null)
                {
                    // Fetch the product by ID
                    var product = await appDbContext.Products.FindAsync(itemDTO.ProductId);
                    if (product != null)
                    {
                  
                        if (itemDTO.Quantity > product.StockQuantity)
                        {
                            return BadRequest($"Insufficient stock for product ID {itemDTO.ProductId}. Available: {product.StockQuantity}");
                        }

                        // Update the existing order item
                        existingOrderItem.ProductId = itemDTO.ProductId;
                        existingOrderItem.Quantity = itemDTO.Quantity;
                        existingOrderItem.UnitPrice = product.Price; 

                        product.StockQuantity -= itemDTO.Quantity;
                    }
                    else
                    {
                        notFoundProducts.Add(itemDTO.ProductId); 
                    }
                }
                else
                {
                    return BadRequest($"Order item with ID {itemDTO.Id} not found in the order.");
                }
            }
           
            //appDbContext.Entry(orderEntity).State = EntityState.Modified;
            await appDbContext.SaveChangesAsync();

            // Map updates for response
            var orderItemDtos = existingOrder.OrderItems.Select(oi => new OrderItemDTO
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,

            }).ToList();

            // Prepare the response DTO
            var updatedOrderDto = new UpdateOrderDTO
            {
                Id = existingOrder.Id,
                CustomerName = existingOrder.CustomerName,
                OrderItems = orderItemDtos
            };

            return Ok(updatedOrderDto);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var orderEntity = await appDbContext.Orders.FindAsync(id);

            if (orderEntity == null)
            {
                return NotFound();
            }

            appDbContext.Orders.Remove(orderEntity);
            await appDbContext.SaveChangesAsync();

            return Ok(orderEntity);

        }


    }
}
