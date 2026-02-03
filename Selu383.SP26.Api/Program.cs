using Microsoft.EntityFrameworkCore;
using Selu383.SP26.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get the connection string from configuration
// On your laptop: This pulls from appsettings.Development.json (LocalDB)
// On GitHub/Azure: This pulls from the Environment Variables/Secrets (Azure SQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add Entity Framework Core with SQL Server
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Migrate and seed the database on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;           //777
    var context = services.GetRequiredService<DataContext>();

    // This is critical: it creates the tables and data on the Azure DB during the GitHub Test run
    context.Database.Migrate();
    SeedData.Initialize(context);
}

app.Run();

// Required for automated integration tests
public partial class Program { }