using Exemplo.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var server = builder.Configuration["DBServer"] ?? "ms-sql-server";
var port = builder.Configuration["DBPort"] ?? "1433";
var user = builder.Configuration["DBUser"] ?? "SA";
var password = builder.Configuration["DBPassword"] ?? "123@Mudar";
var database = builder.Configuration["Database"] ?? "Exemplo";

builder.Services.AddDbContext<ExemploContext>(options =>
    options.UseSqlServer($"Server={server},{port};Initial Catalog={database};User ID={user};Password={password};Encrypt=False"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

PrepDB.PrepPopulation(app);    

app.Run();
