namespace AnimeStore.Data;
public class Product
{
    public int ProductId { get; set; } // ef automatically redobnizes this as the primary key
    public string Sku { get; set; } = "";
    public string ShortName { get; set; } = "";
    public string LongName { get; set; } = "";
    public string Description { get; set; } = "";
    public string? MainImagePath { get; set; } 
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public List<ProductVariant> Variants { get; set; } = new();
    public List<ProductImage> Images { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();


}