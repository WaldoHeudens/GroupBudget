namespace GroupBudget_Web.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } =  string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime Deleted { get; set; } = DateTime.MaxValue;
    }
}
