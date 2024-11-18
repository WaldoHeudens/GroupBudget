using GroupBudget_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupBudget_Web.Data
{
    public class SeedDataContext
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();


            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "?", Description = "?", Deleted = DateTime.Now },
                    new Category { Name = "Test", Description = "TestDescription"});
                context.SaveChanges();
            }

            if (!context.Projects.Any())
            {
                Category defaultCategory = context.Categories.FirstOrDefault(c => c.Name == "?");
                context.Projects.AddRange(
                    new Project { Name="?", Description="?", Deleted=DateTime.Now, CategoryId = defaultCategory.Id },
                    new Project { Name="Test", Description="Test", Category = defaultCategory}
                    );
                context.SaveChanges();
            }
        }
    }
}
