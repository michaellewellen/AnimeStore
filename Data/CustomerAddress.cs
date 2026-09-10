namespace AnimeStore.Data;

public class CustomerAddress
{
    public int CustomerAddressId { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public string RecipientName { get; set; } = "";
    
    public string Address1 { get; set; } = "";
    public string? Address2 { get; set; }
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string ZipCode { get; set; } = "";

    public bool IsPrimary { get; set; }
}