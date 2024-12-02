using GroupBudget_Web.APIModels;
using GroupBudget_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GroupBudget_Web.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly SignInManager<GroupBudgetUser> _signInManager;

        public AccountsController(SignInManager<GroupBudgetUser> signInManager) 
        { 
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("/api/login")]
        public async Task<ActionResult<Boolean>> PostAccount([FromBody] LoginModel @login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Name, login.Password, true, lockoutOnFailure:false);

            return result.Succeeded;
        }

    }

}
