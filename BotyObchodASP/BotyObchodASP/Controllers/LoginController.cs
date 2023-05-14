using BotyObchodASP.Models;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;

namespace BotyObchodASP.Controllers
{
    public class LoginController : Controller
    {
        private LoginService loginService = new();
        [HttpGet]
        public IActionResult Index()
        {
            return View(new TbAdmin());
        }
        [HttpPost]
        public IActionResult Index(TbAdmin model)
        {
            if (loginService.Authenticate(model))
            {
                var tok = JwtBuilder.Create().WithAlgorithm(new HMACSHA256Algorithm()).WithSecret(loginService._SECRET).Encode();

                this.HttpContext.Session.SetString("login", tok);
                return RedirectToAction("Index", "Admin");
            }

            return View(model);
        }   

        public IActionResult Logout()
        {

            this.HttpContext.Session.Remove("login");
            return RedirectToAction("Index", "Home");
        }


    }
}
