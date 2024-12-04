using System.ComponentModel.DataAnnotations;

namespace GroupBudget_Web.Models
{
    public class Language
    {
        [Key]
        [StringLength(2)]
        [Display (Name ="Code")]
        public string Code { get; set; } = "? ";

        [Required]
        [Display (Name = "Language")]
        public string Name { get; set; } = "?";

        [Required]
        public bool IsSystemLanguage { get; set; }= false;


        static public List<Language> Languages { get; set;} = new List<Language> ();

    }
}
