using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GroupBudget_Web.Models;

namespace GroupBudget_Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GroupBudget_Web.Models.Project> Projects { get; set; } = default!;
        public DbSet<GroupBudget_Web.Models.Category> Categories { get; set; } = default!;
    }
}
