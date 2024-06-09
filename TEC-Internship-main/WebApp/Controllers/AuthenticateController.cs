/*using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;
using System.Diagnostics;

namespace WebApp.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly ILogger<AuthenticateController> _logger;

        public AuthenticateController(ILogger<AuthenticateController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(Login login)
        {
            if (ModelState.IsValid)
            {
                if (login.username == "admin" && login.password == "admin")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View("Login", login);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
*/