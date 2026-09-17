using Microsoft.AspNetCore.Identity;
namespace AnimeStore.Data;

public class Customer
{
    public int CustomerId { get; set; }
    
    public string? Email { get; set; } 
    public string? FirstName { get; set; } 
    public string? LastName { get; set; } 
    
    public string? UserId { get; set; }
    public IdentityUser? User{ get; set; }    

    public List<CustomerAddress> Addresses { get; set; } = new()!;
}