using Selu383.SP26.Api.Models;

namespace Selu383.SP26.Api.Data
{
    public static class SeedData
    {
        public static void Initialize(DataContext context)
        {
            if (!context.Locations.Any())
            {
                context.Locations.AddRange(
                    new Location { Name = "Coffee Shop 1", Address = "123 Main St", TableCount = 5 },
                    new Location { Name = "Coffee Shop 2", Address = "456 Elm St", TableCount = 8 },
                    new Location { Name = "Coffee Shop 3", Address = "789 Oak St", TableCount = 10 }
                );
                context.SaveChanges();
            }
        }
    }
}