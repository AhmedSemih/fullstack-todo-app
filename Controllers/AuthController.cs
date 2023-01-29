using fullstack_todo_app.Entities;
using fullstack_todo_app.Models;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;

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
                    return RedirectToAction(nameof(Index), "Home");
                }
            }

            return View(model);
        }        
    }
}

