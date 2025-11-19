using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Services;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Interfaces.Repositories;
using MyApp.Infrastructure;
using MyApp.Infrastructure.Repositories;
using Serilog;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Logger Configuration
builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
builder.Logging.AddSerilog(Log.Logger);

//builder.Host.UseSerilog();

// Add DbContext
//if (builder.Environment.IsDevelopment())
//{
//    builder.Services.AddDbContext<AppDbContext>(options =>
//        options.UseInMemoryDatabase("MyAppDb"));
//    builder.Services.AddDbContext<ControlDbContext>(options =>
//        options.UseInMemoryDatabase("ControlDb"));
//}
//else
//{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("MyAppDbConnection")));
    builder.Services.AddDbContext<ControlDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ControlDbConnection")));
//}

// Register UnitOfWork
builder.Services.AddScoped<IAppUnitOfWork>(provider => provider.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IControlUnitOfWork>(provider => provider.GetRequiredService<ControlDbContext>());

// Scrutor for scanning and registering repositories (optional)
builder.Services.Scan(scan => scan
    .FromAssemblyOf<ProductRepository>()
    .AddClasses(classes => classes.InNamespaces("MyApp.Infrastructure.Repositories"))
    .AsImplementedInterfaces()
    .WithScopedLifetime());
// Scrutor for scanning and registering ApplicationServices
builder.Services.Scan(scan => scan
    .FromAssemblyOf<ProductService>()
    .AddClasses(classes => classes.InNamespaces("MyApp.ApplicationServices.Services"))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Add Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// ? Add Swagger Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "MyApp API", Version = "v1" });
});

var app = builder.Build();

// ? Enable Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyApp API v1");
        options.RoutePrefix = string.Empty; // Swagger at root URL
    });
}

app.UseDeveloperExceptionPage();
app.MapControllers();

// Replace the MapGet lambda with a call to the local function
app.MapGet("Stream", (IProductRepository productRepo, CancellationToken cancellationToken) =>
{
    return productRepo.StreamAllAsync(cancellationToken);
});

app.Run();