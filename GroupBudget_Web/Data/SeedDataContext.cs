using GroupBudget_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace GroupBudget_Web.Data
{
    public class SeedDataContext
    {
        public static async void Initialize(ApplicationDbContext context, UserManager<GroupBudgetUser> userManager)
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();

            GroupBudgetUser dummyUser = null;
            GroupBudgetUser testUser = null;

            if (context.Users.FirstOrDefault(u => u.Id == "?") == null)
            {
                dummyUser = new GroupBudgetUser { Id = "?", UserName = "?", FirstName = "?", LastName="?", Email="?@?", PasswordHash="?", LockoutEnabled = true };
                testUser = new GroupBudgetUser { UserName = "Test", FirstName = "Test", LastName = "Test", Email = "Test@Test.be" };
                context.Users.Add(dummyUser);
                context.SaveChanges();
                var result = await userManager.CreateAsync(testUser, "Xxx!12345");
            }

            dummyUser = context.Users.FirstOrDefault(u => u.UserName == "?");
            testUser = context.Users.FirstOrDefault(u => u.UserName == "Test");

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new IdentityRole { Id = "User", Name = "User", NormalizedName = "USER" },
                    new IdentityRole { Id = "UserAdmin", Name = "UserAdmin", NormalizedName = "USERADMIN" }
                    );
                context.SaveChanges() ;
                context.UserRoles.Add(new IdentityUserRole<string> { RoleId = "User", UserId = "?" });
                context.SaveChanges();
            }



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
                    new Project { Name="?", Description="?", Deleted=DateTime.Now, CategoryId = defaultCategory.Id, StartedById = "?" },
                    new Project { Name="Test", Description="Test", Category = defaultCategory, StartedById = dummyUser.Id}
                    );
                context.SaveChanges();
            }
        }
    }
}
