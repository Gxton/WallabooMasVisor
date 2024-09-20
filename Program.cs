using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wallabo.Entities;
using Wallaboo.Data;
using Wallaboo.Services;
using MercadoPago.Client.CardToken;
using MercadoPago.Client.Customer;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using Wallaboo.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Ofi") ?? throw new InvalidOperationException("Connection string not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
    //.AddErrorDescriber<MyErrorDescriber>();
    
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<CardTokenClient>();
builder.Services.AddScoped<CustomerClient>();
builder.Services.AddScoped<PaymentClient>();
builder.Services.AddScoped<IMercadoPagoService, MercadoPagoService>();


//Servicios
builder.Services.AddTransient<IServiceTenant, ServiceTenant>();
builder.Services.AddHttpClient<IMercadoPagoService, MercadoPagoService>((serviceProvider, httpClient) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var accessToken = configuration["MercadoPagoConfig:AccessToken"];

    httpClient.BaseAddress = new Uri("https://api.mercadopago.com/v1/");
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    httpClient.DefaultRequestHeaders.Add("User-Agent", "Wallaboo");
    //httpClient.DefaultRequestHeaders.Add("User-Agent", "MercadoPagoApp");

    // Añadir el token de acceso a los headers de autorización si es necesario
    // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
});
// Configurar Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MercadoPago API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MercadoPago API v1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Anuncios}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
