namespace AnimeStore.Data;

public class Tag
{
    public int TagId { get; set; }  
    public string TagName { get; set;} = "";
    
    public List<Product> Products {get; set; } = new();
}