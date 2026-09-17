namespace AnimeStore.Data;
public class Product
{
    public int ProductId { get; set; } // ef automatically redobnizes this as the primary key
    
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public string Sku { get; set; } = "";
    public string ShortName { get; set; } = "";
    public string LongName { get; set; } = "";
    public string Description { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public List<ProductVariant> Variants { get; set; } = new();
    public List<ProductImage> Images { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();


    public string Serialize()
    {
        var json = System.Text.Json.JsonSerializer.Serialize(this, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        });
        return json;
    }

}