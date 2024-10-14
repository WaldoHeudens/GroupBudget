using GroupBudget_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBudget_WPF
{
    internal class Seeder
    {
        public Seeder(GB_Context context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "?", Description = "?", Deleted = DateTime.Now },
                    new Category { Name = "Road Trip", Description = "Road Trip" });
                context.SaveChanges();
            }

            if (!context.Projects.Any())
            {
                context.Projects.AddRange(
                    new Project { Name = "?", Description = "?", Deleted = DateTime.Now, CategoryId = 1 },
                    new Project
                    {
                        Name = "Route 66",
                        Description = "Route 66 Road Trip",
                        StartDate = DateTime.Now,
                        EstimatedBudget = 10000,
                        CategoryId = 1
                    });
                context.SaveChanges();
            }

            if (!context.Persons.Any())
            {
                context.Persons.AddRange(
                    new Person { Name = "?", FirstName = "?", LastName = "?", Deleted = DateTime.Now },
                    new Person { Name = "TestJefke", FirstName = "Jefke", LastName = "Test" },
                    new Person { Name = "TestMieke", FirstName = "Mieke", LastName = "Test"});
                context.SaveChangesAsync();
            }
        }
    }
}
