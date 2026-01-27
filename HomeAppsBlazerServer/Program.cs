using HomeAppsBlazerServer.Components;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Shopping;
using HomeAppsBlazerServer.Servcies.Auto;
using HomeAppsBlazerServer.Servcies.Chore;
using HomeAppsBlazerServer.Servcies.Shopping;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazorBootstrap();

builder.Services.AddDbContext<MyDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .LogTo(Console.WriteLine, LogLevel.Information)  // See all queries  Just TESTING
        .EnableSensitiveDataLogging() // See the actual values being used in the queries  Just TESTING
    );

builder.Services.AddScoped<IShoppingServices, ShoppingServices>();
builder.Services.AddScoped<IKidsChorseKidsServices, KidsChorseKidsServices>();
builder.Services.AddScoped<IChoresChoresServices, ChoresChoresServices>();
builder.Services.AddScoped<ILocationChoresServices, LocationChoresServices>();
builder.Services.AddScoped<ChoresListItemChoresServices, ChoresListItemChoresServices>();
builder.Services.AddScoped<CarService, CarService>();
builder.Services.AddScoped<IGasStationService, GasStationService>();
builder.Services.AddScoped<GasTypeService, GasTypeService>();
builder.Services.AddScoped<MileageEntryService, MileageEntryService>();
builder.Services.AddScoped<ItemBrand, ItemBrand>();


builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
    options.DetailedErrors = true;
});

builder.Services.AddSignalR(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
    options.KeepAliveInterval = TimeSpan.FromSeconds(30);
    options.EnableDetailedErrors = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();