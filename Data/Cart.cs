namespace AnimeStore.Data;

public class Cart
{
    public int CartId { get; set; }

    public string GuestToken { get; set; } = "";
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public List<CartItem> Items { get; set; } = new(); 
}