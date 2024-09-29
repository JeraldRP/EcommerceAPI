using EcommerceProductManagement.Data;
using EcommerceProductManagement.Models;
using EcommerceProductManagement.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProductManagement.Services
{
    public class ProductService
    {
        private readonly AppDbContext appDbContext;

        public ProductService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
  
        public async Task<List<Product>> GetProductByCategory(int categoryID)
        {
            return await appDbContext.Products
                .Where(p => p.Categories.Any(c => c.Id == categoryID))
                .ToListAsync();
        }
     
        public async Task<List<Order>> GetOrdersLastMonth()
        {
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            return await appDbContext.Orders
                .Where(o => o.OrderDate >= oneMonthAgo)
                .ToListAsync();
        }
   
        public async Task<Dictionary<int, decimal>> GetTotalSalesOfProduct()
        {
            return await appDbContext.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Select( g => new
                {
                    ProductId = g.Key,
                    TotalSales = g.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .ToDictionaryAsync(x => x.ProductId, x => x.TotalSales);
        }

        public async Task<List<ProductSalesDTO>> GetTopFiveProduct()
        {
            var productSales = await GetTotalSalesOfProduct();
            var salesDict = productSales.ToDictionary(ps => ps.Key, ps => ps.Value);
            var productIds = salesDict.Keys.ToList(); 

            // Fetch products from the db
            var products = await appDbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            // Order by total sales 
            var topProducts = products
                .OrderByDescending(p => salesDict[p.Id])
                .Take(5)
                .Select(p => new ProductSalesDTO
                 {
                     ProductName = p.Name,            
                     TotalSales = salesDict[p.Id]     
                 }).ToList();

            return topProducts;
        }

    }
}
