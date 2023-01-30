using fullstack_todo_app.Entities;
using fullstack_todo_app.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;
using System.Security.Claims;

namespace fullstack_todo_app.Controllers
{
    public class AuthController : Controller
    {
        private DBContext _dbContext;
        private IConfiguration _configuration;

        public AuthController(DBContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                string saltedPassword = model.Password + md5Salt;
                string hashedPassword = saltedPassword.MD5();

                if (_dbContext.Users.Any(x => x.Email.ToLower() == model.Email.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Email), "This email is already exists.");
                    return View(model);
                }

                User user = new()
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = hashedPassword
                };

                _dbContext.Users.Add(user);
                int affedtedRows = _dbContext.SaveChanges();

                if (affedtedRows == 0)
                {
                    ModelState.AddModelError("", "Somethings went wrong.");
                }
                else
                {
                    return RedirectToAction(nameof(Login));
                }
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                string saltedPassword = model.Password + md5Salt;
                string hashedPassword = saltedPassword.MD5();

                User user = _dbContext.Users.FirstOrDefault(x => x.Email.ToLower() == model.Email.ToLower() && x.Password == hashedPassword);
                if (user != null)
                {
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username)
                    };

                    ClaimsIdentity identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(principal);

                    return RedirectToAction(nameof(Index),"Home");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong email or password.");
                }
            }

            return View(model);
        }

        public IActionResult Logout() {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}

