using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapGet("/products", async (AppDbContext db) =>
    await db.Products.ToListAsync());

app.MapGet("/products/{id}", async (Guid id, AppDbContext db) =>
    await db.Products.FindAsync(id) is Product p ? Results.Ok(p) : Results.NotFound());

app.MapPost("/products", async (Product product, AppDbContext db) =>
{
    product.Id = Guid.NewGuid();
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});

app.MapPut("/products/{id}", async (Guid id, Product input, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();
    product.Name = input.Name;
    product.Price = input.Price;
    product.Stock = input.Stock;
    await db.SaveChangesAsync();
    return Results.Ok(product);
});

app.MapDelete("/products/{id}", async (Guid id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();
    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();