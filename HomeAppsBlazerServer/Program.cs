using HomeAppsBlazerServer.Components;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Servcies.Auto;
using HomeAppsBlazerServer.Servcies.Chore;
using HomeAppsBlazerServer.Servcies.Shopping;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); //Need to use the newest and coolest stuff with blazer and .net 8

builder.Services.AddBlazorBootstrap();

builder.Services.AddDbContext<MyDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IShoppingServices, ShoppingServices>();

builder.Services.AddScoped<IKidsChorseKidsServices, KidsChorseKidsServices>();

builder.Services.AddScoped<IChoresChoresServices, ChoresChoresServices>();

builder.Services.AddScoped<ILocationChoresServices, LocationChoresServices>();

builder.Services.AddScoped<ChoresListItemChoresServices, ChoresListItemChoresServices>();

builder.Services.AddScoped<CarService, CarService>();

builder.Services.AddScoped<IGasStationService, GasStationService>();

builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
    options.DetailedErrors = true;
});

builder.Services.AddSignalR(options =>
{
    // Increase timeout to handle slower mobile connections
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
    options.KeepAliveInterval = TimeSpan.FromSeconds(30);

    // Add transport fallback options
    options.EnableDetailedErrors = true;
});


builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
        options.HandshakeTimeout = TimeSpan.FromSeconds(30);
    });


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
