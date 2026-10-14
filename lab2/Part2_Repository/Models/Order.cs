namespace Part2_Repository.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
}
