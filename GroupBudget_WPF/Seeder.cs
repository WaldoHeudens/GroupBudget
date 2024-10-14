using GroupBudget_WPF.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Interop;
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
                context.SaveChanges();
            }

            if (!context.PersonsProjects.Any()) {
                int projectId = context.Projects.First(p => p.Name == "Route 66").Id;
                context.PersonsProjects.AddRange(
                    new PersonProject { ProjectId = projectId, PersonId = context.Persons.FirstOrDefault(person => person.Name == "TestJefke").Id },
                    new PersonProject { ProjectId = projectId, PersonId = context.Persons.FirstOrDefault(person => person.Name == "TestMieke").Id }
                    );
                context.SaveChanges();
            }

            if (!context.Budgets.Any())
            {
                Person person = context.Persons.FirstOrDefault(person => person.Name == "TestJefke");
                Project project = context.Projects.FirstOrDefault(p => p.Name == "Route 66");
                context.Budgets.AddRange(
                    new Budget { Description = "?", BudgetType = BudgetType.Income, Amount = 0, PersonId = 1, ProjectId = 1 },
                    new Budget { Description = "Camper Rental", BudgetType = BudgetType.Expense, Amount = 2.500, ProjectId = project.Id, PersonId = person.Id },
                    new Budget { Description = "Betaling", BudgetType = BudgetType.Income, Amount=1000, ProjectId = project.Id, 
                                    PersonId = context.Persons.FirstOrDefault(p => p.Name=="TestMieke").Id}
                    );
                context.SaveChanges();
            }
        }
    }
}
