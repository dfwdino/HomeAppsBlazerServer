using HomeAppsBlazerServer.Components;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Servcies;
using HomeAppsBlazerServer.Servcies.Chore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); //Need to use the newest and coolest stuff with blazer and .net 8

builder.Services.AddBlazorBootstrap();

builder.Services.AddDbContext<MyDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IShoppingServices, ShoppingServices>();
builder.Services.AddScoped<KidsChorseKidsServices, KidsChorseKidsServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); //Need to use the newest and coolest stuff with blazer and .net 8

app.Run();
