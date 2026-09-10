namespace AnimeStore.Data;

public class Cart
{
    public int CartId { get; set; }

    public int CutomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public List<CartItem> Items { get; set; } = new()!; 
}