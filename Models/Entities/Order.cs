namespace EcommerceProductManagement.Models.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public DateTime OrderDate { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
