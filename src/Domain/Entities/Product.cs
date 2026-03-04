namespace Domain.Entities;

public class Product
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public ICollection<Item> Items { get; set; } = new List<Item>();
    public int Stock { get; set; }
}