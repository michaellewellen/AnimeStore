namespace AnimeStore.Data;

public class Order
{
    public int OrderId { get; set;} 
    
    public int CustomerId { get; set;}
    public Customer Customer { get; set;} = null!;

    public int CustomerAddressId { get; set; }
    public CustomerAddress ShippingAddress { get; set; } = null!;

    public OrderStatus Status { get; set; } = OrderStatus.Placed;
    public DateTime OrderDate   { get; set; }= DateTime.UtcNow;
    
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal OrderTotal { get; set; }

    public List<OrderItem> Items { get; set; } = new()!;
}