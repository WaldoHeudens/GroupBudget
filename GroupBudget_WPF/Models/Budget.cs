using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBudget_WPF.Models
{
    public enum BudgetType { Income, Expense}
    public class Budget
    {
        public int Id { get; set; }
        [ForeignKey("Projects")]
        public int ProjectId { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }  
        public BudgetType BudgetType { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [ForeignKey ("Persons")]
        public int PersonId { get; set; } = 1;
        public bool IsDeleted { get; set; } = false;


        public Person Person { get; set; }
        public Project Project { get; set; }
        

    }
}
