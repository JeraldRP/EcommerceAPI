namespace EcommerceProductManagement.Models.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        //For many to many
        public ICollection<Category> Categories { get; set; } = new List<Category>();

    }
}
