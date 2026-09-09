using System.ComponentModel;

namespace AnimeStore.Data;

public class Review
{
    public int ReviewId { get; set; }   
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public string ReviewerName { get; set; } = "";
    public int Rating { get; set; }
    public string Comment { get; set; } = "";
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}