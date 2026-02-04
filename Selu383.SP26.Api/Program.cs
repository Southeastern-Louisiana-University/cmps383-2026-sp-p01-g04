using Microsoft.EntityFrameworkCore;
using Selu383.SP26.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// REQUIREMENT: Use SQL Server and the connection string name "DataContext"
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext")));

var app = builder.Build();

// REQUIREMENT: The database should be programmatically created and migrated on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();

    // This applies any pending migrations and creates the DB if it doesn't exist
    context.Database.Migrate();

    // REQUIREMENT: Prepopulate with seed data
    SeedData.Initialize(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// REQUIREMENT: Required for automated integration tests
public partial class Program { }   