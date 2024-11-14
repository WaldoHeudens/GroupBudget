using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace GroupBudget_Web.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Display (Name="Started")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Display (Name="Will end")]
        [Required]
        [DataType (DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.MaxValue;

        [Display (Name="Estimated Budget")]
        public Decimal EstimatedBudget { get; set; } = 0;


        public DateTime Deleted { get; set; } = DateTime.MaxValue;

    }
}
