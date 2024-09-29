using EcommerceProductManagement.Models.Entity;

namespace EcommerceProductManagement.Models
{
    public class AddCategoryDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public ICollection<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
