using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    ViewData["Layout"] = "~/Views/Shared/_Layout.cshtml"; // Use layout.cshtml after login
            //}
            //else
            //{
            //    ViewData["Layout"] = "~/Views/Shared/_Layout1.cshtml"; // Use layout1.cshtml before login
            //}
            return View();
        }

        public IActionResult Index1()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
