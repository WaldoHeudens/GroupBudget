using GroupBudget_Web.Data.Migrations;
using GroupBudget_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace GroupBudget_Web.Data
{
    public class SeedDataContext
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<GroupBudgetUser> userManager)
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();

            GroupBudgetUser dummyUser = null;
            GroupBudgetUser testUser = null;
            GroupBudgetUser systemAdmin = null;

            if (!context.Languages.Any())
            {
                context.Languages.AddRange(
                    new Language(),
                    new Language { Code = "en", IsSystemLanguage = true, Name = "English" },
                    new Language { Code = "fr", IsSystemLanguage = true, Name = "français" },
                    new Language { Code = "nl", IsSystemLanguage = true, Name = "Nederlands" },
                    new Language { Code = "de", IsSystemLanguage = false, Name = "Deutsch" }
                    ); 
                context.SaveChanges();
            }

            Language.Languages = context.Languages.Where(l => l.IsSystemLanguage && l.Code != "? ").ToList();

            if (context.Users.FirstOrDefault(u => u.Id == "?") == null)
            {
                dummyUser = new GroupBudgetUser { Id = "?", UserName = "?", FirstName = "?", LastName="?", Email="?@?", PasswordHash="?", LockoutEnabled = true, LanguageCode = "?" };
                testUser = new GroupBudgetUser { UserName = "Test", FirstName = "Test", LastName = "Test", Email = "Test@Test.be", LanguageCode = "nl"};
                systemAdmin = new GroupBudgetUser { UserName = "SystemAdmin", FirstName = "System", LastName = "Admin", Email = "System@Test.be", LanguageCode = "nl" };
                context.Users.Add(dummyUser);
                context.SaveChanges();
                var result = await userManager.CreateAsync(testUser, "Xxx!12345");
                result = await userManager.CreateAsync(systemAdmin, "Xxx!12345");
            }

            Globals.DefaultUser = context.Users.FirstOrDefault(u => u.UserName == "?");
            testUser = context.Users.FirstOrDefault(u => u.UserName == "Test");

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new IdentityRole { Id = "User", Name = "User", NormalizedName = "USER" },
                    new IdentityRole { Id = "UserAdmin", Name = "UserAdmin", NormalizedName = "USERADMIN" },
                    new IdentityRole { Id = "SystemAdmin", Name = "SystemAdmin", NormalizedName = "SYSTEMADMIN" }
                    );
                context.SaveChanges() ;
                context.UserRoles.Add(new IdentityUserRole<string> { RoleId = "User", UserId = "?" });
                context.UserRoles.Add(new IdentityUserRole<string> { RoleId = "SystemAdmin", UserId = systemAdmin.Id });
                context.UserRoles.Add(new IdentityUserRole<string> { RoleId = "UserAdmin", UserId = testUser.Id });

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

            Parameter.AddParameters(context, Globals.DefaultUser);
        }
    }
}
