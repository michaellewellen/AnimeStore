namespace AnimeStore.Data;

public class ProductVariant
{
    public int ProductVariantId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string? ProductColor { get; set; }
    public string? ProductSize { get; set; }

    public decimal RetailPrice { get; set; }
    public decimal WholesalePrice { get; set; }
    public int StockQuantity    { get; set; } 
}