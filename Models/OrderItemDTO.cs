using EcommerceProductManagement.Models.Entity;

namespace EcommerceProductManagement.Models
{
    public class OrderItemDTO
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
