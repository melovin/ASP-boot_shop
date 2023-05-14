using BotyObchodASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BotyObchodASP.Attributes
{
    public class SecuredAttribute : Attribute, IAuthorizationFilter
    {
        private LoginService loginService = new();
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string? tok = context.HttpContext.Session.GetString("login");
            if (tok == null || !loginService.VerifyToken(tok))
            {
                context.Result = new RedirectToActionResult("Index", "Login", new { });
            }
        }
    }
}
