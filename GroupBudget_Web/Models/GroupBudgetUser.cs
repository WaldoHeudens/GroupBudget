using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupBudget_Web.Models
{
    public class GroupBudgetUser:IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required] 
        public string LastName { get; set; }

        [Required]
        [StringLength(2)]
        [ForeignKey("Languages")]
        public string LanguageCode { get; set; } = "?";

    }
}
