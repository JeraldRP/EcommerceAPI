using EcommerceProductManagement.Models.Entity;

namespace EcommerceProductManagement.Models
{
    public class UpdateOrderDTO
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public DateTime OrderDate { get; set; }

        public ICollection<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}
