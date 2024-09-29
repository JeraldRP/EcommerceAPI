namespace EcommerceProductManagement.Models
{
    public class ProductSalesDTO
    {
        public int ProductId  { get; set; }
        public string? ProductName { get; set; } 
        public decimal TotalSales { get; set; }
    }
}
