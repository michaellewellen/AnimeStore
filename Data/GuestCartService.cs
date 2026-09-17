using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace AnimeStore.Data;

public class GuestCartService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IJSRuntime _js;

    public GuestCartService(ApplicationDbContext dbContext, IJSRuntime js)
    {
        _dbContext = dbContext;
        _js = js;
    }

    public async Task<string> GetOrCreateGuestTokenAsync()
    {
        var existing = await _js.InvokeAsync<string?>("cartCookie.get");

        if (!string.IsNullOrEmpty(existing))
        {
            return existing;
        }

        var newToken = Guid.NewGuid().ToString();
        await _js.InvokeVoidAsync("cartCookie.set", newToken);
        return newToken;
    }

    public async Task<Cart> GetOrCreateCurrentCartAsync(string guestToken)
    {
        var cart = await _dbContext.Carts
            .Include(c => c.Items).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Product).ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(c => c.GuestToken == guestToken);

        if (cart is null)
        {
            cart = new Cart { GuestToken = guestToken };
            _dbContext.Carts.Add(cart);
            await _dbContext.SaveChangesAsync();
        }

        return cart;
    }
}