using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GroupBudget_Web.Models
{
    public class GroupBudgetUser:IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required] 
        public string LastName { get; set; }


    }
}
