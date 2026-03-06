using HomeAppsBlazerServer.Components;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Shopping;
using HomeAppsBlazerServer.Servcies.Auto;
using HomeAppsBlazerServer.Servcies.Shopping;
using Microsoft.EntityFrameworkCore;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

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
builder.Services.AddScoped<CarService, CarService>();
builder.Services.AddScoped<IGasStationService, GasStationService>();
builder.Services.AddScoped<GasTypeService, GasTypeService>();
builder.Services.AddScoped<MileageEntryService, MileageEntryService>();
builder.Services.AddScoped<ItemBrand, ItemBrand>();

builder.Services.AddMemoryCache();


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


#region Serilog Configuration

// Temporary: shows Serilog internal errors in the console output window
Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine($"SERILOG INTERNAL ERROR: {msg}"));

var columnOptions = new ColumnOptions();

// Map PK to match your LogID column name
columnOptions.Id.ColumnName = "LogID";

// Remove default Serilog columns that don't exist in your table
columnOptions.Store.Remove(StandardColumn.MessageTemplate);
//columnOptions.Store.Remove(StandardColumn.Properties);

// Add custom columns
columnOptions.AdditionalColumns = new Collection<SqlColumn>
{
    new SqlColumn { ColumnName = "IpAddress", DataType = SqlDbType.NVarChar, DataLength = 50, AllowNull = true },
    new SqlColumn { ColumnName = "CameFrom",  DataType = SqlDbType.NVarChar, DataLength = 50, AllowNull = true }
};

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)          // filter out ASP.NET Core noise
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)  // keep startup messages
    .MinimumLevel.Override("System", LogEventLevel.Warning)             // filter out System noise
    .Enrich.FromLogContext()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Log",
            SchemaName = "dbo",
            AutoCreateSqlTable = false,
            BatchPostingLimit = 1,                        // write immediately instead of batching
            BatchPeriod = TimeSpan.FromSeconds(1)         // flush every second
        },
        columnOptions: columnOptions
    )
    .CreateLogger();

builder.Host.UseSerilog();

// Temporary test log - if this doesn't appear in the DB table, Serilog config is the issue
//Log.Information("Serilog test - application starting up");

#endregion End of Serilog Configuration


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

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush(); // ensure all buffered logs are written on shutdown
}