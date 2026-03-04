using ECommerceWeb.CustomMiddleware;
using ECommerceWeb.Extentions;
using ECommerceWeb.Factory;
using ECommerece.Domain.Contracts;
using ECommerece.Domain.IdentityModule;
using ECommerece.presistance.Data.DbContexts;
using ECommerece.presistance.DataSeed;
using ECommerece.presistance.IdentityData.DataSeed;
using ECommerece.presistance.IdentityData.DbContexts;
using ECommerece.presistance.Repository;
using ECommerece.Service;
using ECommerece.Service.MappingProfiles;
using ECommerece.ServiceAbstraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Reflection.Metadata.Ecma335;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddKeyedScoped<IDataInitializer, DataInitializer>("Default");
builder.Services.AddKeyedScoped<IDataInitializer, IdentityDataInitializer>("Identity");
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddAutoMapper(x => x.AddProfile<ProductProfile>());
//builder.Services.AddTransient<ProductPictureUrlResolver>();
builder.Services.AddAutoMapper(typeof(ServiceAssemplyReference).Assembly);
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<ICashService, CashService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
}
);
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<ICashRepository, CashRepository>();
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>();
builder.Services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<StoreIdentityDbContext>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
});

builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
        ValidAudience = builder.Configuration["JwtOptions:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:SecretKey"]!)),
    };
});

var app = builder.Build();

#region data seeding
await app.MigrateDatabaesAsync();
await app.MigrateIdentityDatabaesAsync();
await app.SeedDataAsync();
await app.SeedIdentityDataAsync();
#endregion

#region PipeLine

//app.Use(async (context, next) =>
//{
//    try
//    {
//        await next.Invoke(context);
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"An error occurred: {ex.Message}");
//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

//        // Return a JSON response with the error message
//        await context.Response.WriteAsJsonAsync(new
//        {
//            statusCode = StatusCodes.Status500InternalServerError,
//            Error = ex.Message,
//        });
//    }
//});

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
#endregion
