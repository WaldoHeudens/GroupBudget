using GroupBudget_Web.Data;
using GroupBudget_Web.Models;

namespace GroupBudget_Web.Services
{
    public interface IMyUser
    {
        public GroupBudgetUser User { get; }
    }

    public class MyUser : IMyUser
    {
        ApplicationDbContext _context;
        IHttpContextAccessor _httpContext;

        public GroupBudgetUser User { get { return GetUser(); } }


        public MyUser(ApplicationDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public GroupBudgetUser GetUser()
        {
            string name = _httpContext.HttpContext.User.Identity.Name;
            if (name == null || name == "")
                return Globals.DefaultUser;
            else
                return _context.Users.FirstOrDefault(u => u.UserName == name);
        }
    }
}
