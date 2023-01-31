using fullstack_todo_app.Entities;
using fullstack_todo_app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace fullstack_todo_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext _dbContext;

        public HomeController(ILogger<HomeController> logger, DBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            Guid userId = new(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var todos = _dbContext.Todos.Where(x => x.UserId == userId && x.Status == false).OrderBy(x => x.Deadline).ToList();
            ViewBag.todos = todos;
            ViewBag.now = DateTime.Now;

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateTodo(TodoViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid userId = new(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                Todo todo = new()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Deadline = model.Deadline,
                    UserId = userId,
                    Status = model.Status
                };

                _dbContext.Todos.Add(todo);
                int affectedRows = _dbContext.SaveChanges();

                if (affectedRows == 0)
                {
                    ModelState.AddModelError("", "Somethings went wrong.");
                    return View(nameof(Index), model);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        public IActionResult CompleteTodo(TodoViewModel model)
        {
            Todo todo = _dbContext.Todos.Where(x=>x.Id== model.Id).FirstOrDefault();
            todo.Status = model.Status;
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}