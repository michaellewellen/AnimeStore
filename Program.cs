using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AnimeStore.Data;
using AnimeStore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register the database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

// Register Identity, wired to our DbContext
// note the password requiredment modifications
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    if (!dbContext.Products.Any())
    {
        // insert dummy data to the database
        // Categories
        var apparel = new Category { Name = "Apparel" };
        var jackets = new Category { Name = "Jackets", ParentCategory = apparel };
        var figures = new Category { Name = "Figures"};

        // Suppliers
        var supplier1 = new Supplier
        {
            SupplierName = "Kyoto Wholesale Imports",
            ContactName = "Aiko Tanaka",
            Address1 = "123 Import Way",
            Address2 = "",
            City = "Ephraim",
            State = "UT",
            ZipCode = "84627",
            PhoneNumber = "555-0101",
            Email = "aiko@kyotowholesale.example"            
        };
        var supplier2 = new Supplier
        {
            SupplierName = "Big & Tall Anime Apparel Co",
            ContactName = "Becky Larsen",
            Address1 = "456 Warehouse Rd",
            Address2 = "Suite 2",
            City = "Salt Lake City",
            State = "UT",
            ZipCode = "84101",
            PhoneNumber = "555-0202",
            Email = "becky@bigtallanime.example"
        };
        var product1 = new Product
        {
            Sku = "JCK-001",
            ShortName = "Ronin Windbreaker",
            LongName = "Ronin Windbreaker - Water Resistant Anime Jacket",
            Description = "A sleek windbreaker inspired by classic samurai anime silhouettes.",
            IsActive = true,
            Category = jackets,
            Supplier = supplier2
        };

        var product2 = new Product
        {
            Sku = "FIG-001",
            ShortName = "Sakura Figure",
            LongName = "Sakura Blossom Collectible Figure - 8in",
            Description = "A finely detailed collectible figure, hand-painted.",
            IsActive = true,
            Category = figures,
            Supplier = supplier1
        };

        var product3 = new Product
        {
            Sku = "JCK-002",
            ShortName = "Big & Tall Hoodie",
            LongName = "Big & Tall Comfort Fit Anime Hoodie",
            Description = "Extra roomy hoodie cut for a taller, broader fit.",
            IsActive = true,
            Category = jackets,
            Supplier = supplier2
        };

        var product4 = new Product
        {
            Sku = "FIG-002",
            ShortName = "Mecha Model Kit",
            LongName = "Buildable Mecha Model Kit - Snap Fit",
            Description = "No glue needed, snap-together mecha model.",
            IsActive = true,
            Category = figures,
            Supplier = supplier1
        };

        var product5 = new Product
        {
            Sku = "COOL-001",
            ShortName = "Anime Cooler",
            LongName = "Insulated Cooler with Anime Print",
            Description = "Keeps drinks cold, looks great at conventions.",
            IsActive = true,
            Category = apparel,
            Supplier = supplier2
        };

        var variants = new List<ProductVariant>
        {
            new() { Product = product1, ProductSize = "M", ProductColor = "Black", RetailPrice = 59.99m, WholesalePrice = 28.00m, StockQuantity = 20 },
            new() { Product = product1, ProductSize = "XXL", ProductColor = "Black", RetailPrice = 69.99m, WholesalePrice = 34.00m, StockQuantity = 8 },
            new() { Product = product2, ProductSize = null, ProductColor = null, RetailPrice = 34.99m, WholesalePrice = 15.00m, StockQuantity = 40 },
            new() { Product = product3, ProductSize = "XXL", ProductColor = "Gray", RetailPrice = 44.99m, WholesalePrice = 20.00m, StockQuantity = 12 },
            new() { Product = product5, ProductSize = null, ProductColor = "Seafoam", RetailPrice = 39.99m, WholesalePrice = 18.00m, StockQuantity = 15 }
        };

        var images = new List<ProductImage>
        {
            new() { Product = product1, ImagePath = "/images/products/ronin_jacket.png", DisplayOrder = 1 },
            new() { Product = product2, ImagePath = "/images/products/Sakura_Figure.png", DisplayOrder = 1 },
            new() { Product = product3, ImagePath = "/images/products/BT_Hoodie.png", DisplayOrder = 1 },
            new() { Product = product4, ImagePath = "/images/products/Mecha_model.png", DisplayOrder = 1 },
            new() { Product = product5, ImagePath = "/images/products/Anime_cup.png", DisplayOrder = 1 }
        };

        var newArrival = new Tag { TagName = "New Arrival" };
        var bigAndTall = new Tag { TagName = "Big & Tall" };
        product1.Tags.Add(newArrival);
        product3.Tags.Add(bigAndTall);
        product3.Tags.Add(newArrival);

        //Add them all
        dbContext.AddRange(apparel, jackets, figures, supplier1, supplier2);
        dbContext.AddRange(product1, product2, product3, product4, product5);
        dbContext.AddRange(variants);
        dbContext.AddRange(images);
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
