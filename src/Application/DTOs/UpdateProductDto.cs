

namespace Application.DTOs
{
    public class UpdateProductDto
    {
        public string ProductName { get; set; } = null!;

        public int Stock { get; set; }

           public decimal Price { get; set; }
    }
}
