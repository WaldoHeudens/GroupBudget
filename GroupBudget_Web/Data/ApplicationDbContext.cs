using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GroupBudget_Web.Models;
using GroupBudget_Web.ViewModels;

namespace GroupBudget_Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<GroupBudgetUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GroupBudget_Web.Models.Project> Projects { get; set; } = default!;
        public DbSet<GroupBudget_Web.Models.Category> Categories { get; set; } = default!;
        public DbSet<GroupBudget_Web.Models.ProjectMember> ProjectMembers { get; set; } = default!;
        public DbSet<GroupBudget_Web.Models.Language> Languages { get; set; } = default!;
        public DbSet<GroupBudget_Web.Models.Parameter> Parameters { get; set; } = default!;
    }
}
