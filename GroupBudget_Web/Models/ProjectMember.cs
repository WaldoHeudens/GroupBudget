using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupBudget_Web.Models
{
    public class ProjectMember
    {
        public int Id { get; set; }

        [ForeignKey("Projects")]
        public int ProjectId { get; set; } = 1;

        [Display (Name = "Member")]
        [ForeignKey("GroupBudgetUser")]
        public string MemberId { get; set; } = "?";
 
        [Display(Name = "Added to project")]
        public DateTime BecameMemberDate { get; set; } = DateTime.Now;

        [Display(Name = "Added by")]
        public string DoneById { get; set; } = "?";          // UserId of creator

        [Display(Name = "Remark")]
        public string Remark { get; set; } = "";
        public DateTime deleted { get; set; } = DateTime.MaxValue;


        [Display(Name = "Project")]
        public Project? Project { get; set; }
        [Display(Name = "Member")]
        public GroupBudgetUser? Member { get; set; }
    }
}
