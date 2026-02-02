using ECommerceWeb.Extentions;
using ECommerece.Domain.Contracts;
using ECommerece.presistance.Data.DbContexts;
using ECommerece.presistance.DataSeed;
using ECommerece.presistance.Repository;
using ECommerece.Service;
using ECommerece.Service.MappingProfiles;
using ECommerece.ServiceAbstraction;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IDataInitializer, DataInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddAutoMapper(x => x.AddProfile<ProductProfile>());
//builder.Services.AddTransient<ProductPictureUrlResolver>();
builder.Services.AddAutoMapper(typeof(ServiceAssemplyReference).Assembly);
builder.Services.AddScoped<IProductService, ProductService>();
var app = builder.Build();

#region data seeding
await app.MigrateDatabaesAsync();
await app.SeedDataAsync();
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
