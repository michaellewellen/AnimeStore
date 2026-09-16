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
    if (!dbContext.Customers.Any())
    {
        // Pull back the variants we already seeded, since those old local
        // variables no longer exist — this is a fresh query against real data.
        var variants = dbContext.ProductVariants.Include(v => v.Product).ToList();
        var windbreakerM = variants.First(v => v.Product.Sku == "JCK-001" && v.ProductSize == "M");
        var figure = variants.First(v => v.Product.Sku == "FIG-001");
        var hoodie = variants.First(v => v.Product.Sku == "JCK-002");
        var cooler = variants.First(v => v.Product.Sku == "COOL-001");

        // Customers — one registered, one registered, one guest (no UserId)
        var mike = new Customer { Email = "mike@example.com", FirstName = "Mike", LastName = "Lewellen" };
        var tara = new Customer { Email = "tara@example.com", FirstName = "Tara", LastName = "Dye" };
        var guest = new Customer { Email = "jordan.guest@example.com", FirstName = "Jordan", LastName = "Guest" };

        // Addresses
        var mikeHome = new CustomerAddress
        {
            Customer = mike, RecipientName = "Michael Lewellen",
            Address1 = "123 Anywhere St", City = "Ephraim", State = "UT", ZipCode = "84627",
            IsPrimary = true
        };
        var mikeShipToTara = new CustomerAddress
        {
            Customer = mike, RecipientName = "Tara Dye",
            Address1 = "456 Maple St", City = "Provo", State = "UT", ZipCode = "84601",
            IsPrimary = false
        };
        var taraHome = new CustomerAddress
        {
            Customer = tara, RecipientName = "Tara Dye",
            Address1 = "456 Maple St", City = "Provo", State = "UT", ZipCode = "84601",
            IsPrimary = true
        };
        var guestAddress = new CustomerAddress
        {
            Customer = guest, RecipientName = "Jordan Guest",
            Address1 = "321 Guest Ln", City = "Salt Lake City", State = "UT", ZipCode = "84101",
            IsPrimary = true
        };

        // Carts — one per customer, some active items, one saved-for-later
        var mikeCart = new Cart { Customer = mike };
        var mikeCartItem1 = new CartItem { Cart = mikeCart, ProductVariant = hoodie, Quantity = 1, IsSavedForLater = false };
        var mikeCartItem2 = new CartItem { Cart = mikeCart, ProductVariant = cooler, Quantity = 1, IsSavedForLater = true };

        var taraCart = new Cart { Customer = tara };
        var guestCart = new Cart { Customer = guest };

        // Order 1 — Mike, already shipped, placed 10 days ago
        var order1Items = new List<OrderItem>
        {
            new() { ProductVariant = windbreakerM, Quantity = 1, UnitPrice = windbreakerM.RetailPrice },
            new() { ProductVariant = figure, Quantity = 2, UnitPrice = figure.RetailPrice }
        };
        var order1Subtotal = order1Items.Sum(i => i.Quantity * i.UnitPrice);
        var order1Tax = PricingCalculator.CalculateTax(order1Subtotal);
        var order1Shipping = 10.00m;

        var order1 = new Order
        {
            Customer = mike,
            ShippingAddress = mikeHome,
            Status = OrderStatus.Shipped,
            OrderDate = DateTime.UtcNow.AddDays(-10),
            SubTotal = order1Subtotal,
            TaxAmount = order1Tax,
            ShippingCost = order1Shipping,
            OrderTotal = order1Subtotal + order1Tax + order1Shipping,
            Items = order1Items
        };

        // Order 2 — Guest, just placed today
        var order2Items = new List<OrderItem>
        {
            new() { ProductVariant = hoodie, Quantity = 1, UnitPrice = hoodie.RetailPrice }
        };
        var order2Subtotal = order2Items.Sum(i => i.Quantity * i.UnitPrice);
        var order2Tax = PricingCalculator.CalculateTax(order2Subtotal);
        var order2Shipping = 10.00m;

        var order2 = new Order
        {
            Customer = guest,
            ShippingAddress = guestAddress,
            Status = OrderStatus.Placed,
            OrderDate = DateTime.UtcNow,
            SubTotal = order2Subtotal,
            TaxAmount = order2Tax,
            ShippingCost = order2Shipping,
            OrderTotal = order2Subtotal + order2Tax + order2Shipping,
            Items = order2Items
        };

        dbContext.AddRange(mike, tara, guest);
        dbContext.AddRange(mikeHome, mikeShipToTara, taraHome, guestAddress);
        dbContext.AddRange(mikeCart, taraCart, guestCart);
        dbContext.AddRange(mikeCartItem1, mikeCartItem2);
        dbContext.AddRange(order1, order2);
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

app.MapGet("/products", async (ApplicationDbContext dbContext) =>
{
    var products = await dbContext.Products
        // .Include(p => p.ProductId)
        // .Include(p => p.Category)
        // .Include(p => p.Supplier)
        // .Include(p => p.Variants)
        // .Include(p => p.Images)
        // .Include(p => p.Tags)
        // .Include(p => p.)
        .Where(p => p.IsActive)
        .ToListAsync();

    return Results.Ok(products);
});

app.MapGet("/products/{id:int}", async (int id, ApplicationDbContext dbContext) =>
{
    var product = await dbContext.Products
        // .Include(p => p.ProductId)
        // .Include(p => p.Category)
        // .Include(p => p.Supplier)
        // .Include(p => p.Variants)
        // .Include(p => p.Images)
        // .Include(p => p.Tags)
        .Where(p => p.IsActive)
        .Select(p => new
        {
            p.ProductId,
            p.Sku,
            p.ShortName,
            p.LongName,
            p.Description,
            p.IsActive,
            Category = new { p.Category.CategoryId, p.Category.Name },
            Supplier = new { p.Supplier.SupplierId, p.Supplier.SupplierName },
            Variants = p.Variants.Select(v => new
            {
                v.ProductVariantId,
                v.ProductSize,
                v.ProductColor,
                v.RetailPrice,
                v.WholesalePrice,
                v.StockQuantity
            }),
            Images = p.Images.Select(i => new
            {
                i.ProductImageId,
                i.ImagePath,
                i.DisplayOrder
            }),
            Tags = p.Tags.Select(t => new
            {
                t.TagId,
                t.TagName
            })
        })
        .FirstOrDefaultAsync(p => p.ProductId == id);

    if (product == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(product);
});

app.Run();
