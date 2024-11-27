using System.ComponentModel.DataAnnotations.Schema;

namespace GroupBudget_Web.Models
{
    public class ProjectMember
    {
        public int Id { get; set; }

        [ForeignKey("Projects")]
        public int ProjectId { get; set; } = 1;

        [ForeignKey("GroupBudgetUser")]
        public string MemberId { get; set; } = "?";

        public DateTime BecameMemberDate { get; set; } = DateTime.Now;

        public string DoneById { get; set; } = "?";          // UserId of creator

        public string Remark { get; set; } = "";
        public DateTime deleted { get; set; } = DateTime.MaxValue;


        public Project? Project { get; set; }   
        public GroupBudgetUser? Member { get; set; }
    }
}
