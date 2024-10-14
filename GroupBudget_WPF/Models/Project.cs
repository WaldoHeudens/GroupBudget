using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBudget_WPF.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.MaxValue;
        public Decimal EstimatedBudget { get; set; } = 0;
        public DateTime Deleted { get; set; } = DateTime.MaxValue;

        [ForeignKey ("Category")]
        public int CategoryId { get; set; }


        public Category Category { get; set; } 
    }
}
