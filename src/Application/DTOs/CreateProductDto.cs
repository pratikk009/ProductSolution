namespace Application.DTOs
{
    public class CreateProductDto
    {
        public string ProductName { get; set; } = null!;
        public int Stock { get; set; }
        public decimal Price { get; set; } 

    }
}