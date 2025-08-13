using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class TimeSheetsController : Controller
    {
        private readonly TaskManagementContext db;


        public TimeSheetsController(TaskManagementContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<IActionResult> Index()
        {

            var timeSheets = await db.MstrTimeSheets
                .Include(t => t.User)
                .Include(t => t.Task)
                .Include(t => t.Project)
                .Include(t => t.ApprovedByNavigation)
                .ToListAsync();

            return View(timeSheets);
        }
    }
}