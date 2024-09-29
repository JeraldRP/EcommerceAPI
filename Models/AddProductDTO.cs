using EcommerceProductManagement.Models.Entity;

namespace EcommerceProductManagement.Models
{
    public class AddProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        //For many to many
        public ICollection<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();
    }
}
