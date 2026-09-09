namespace AnimeStore.Data;

public class ProductImage
{
    public int ProductImageId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string ImagePath { get; set; } = "";
    public int DisplayOrder { get; set; }
}