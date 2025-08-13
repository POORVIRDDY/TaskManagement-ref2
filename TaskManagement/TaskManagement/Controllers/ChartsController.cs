using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    [SessionCheck]
    public class ChartsController : Controller
    {
        private readonly TaskManagementContext _context;

        public ChartsController(TaskManagementContext context)
        {
            _context = context;
        }

        public IActionResult ProjectManager()
        {
            //// Fetch active users grouped by role
            //var roleData = _context.MstrUsers
            //    .Where(u => u.IsUserActivated == true)
            //    .GroupBy(u => u.Role.Name)
            //    .Select(g => new
            //    {
            //        RoleName = g.Key,
            //        UserCount = g.Count()
            //    })
            //    .ToList();

            //ViewBag.Labels = roleData.Select(r => r.RoleName).ToList();
            //ViewBag.Values = roleData.Select(r => r.UserCount).ToList();


            // Fetch active and inactive projects
            var projectData = _context.MstrProjects
                 .Where(p => p.Status != null)
                .GroupBy(p => p.Status)
                .Select(g => new
                {
                    ProjectStatus = g.Key , /*"Active" : "Inactive",*/
                    ProjectCount = g.Count()
                })
                .ToList();

            ViewBag.ProjectLabels = projectData.Select(p => p.ProjectStatus).ToList();
            ViewBag.ProjectValues = projectData.Select(p => p.ProjectCount).ToList();

            // Fetch task statuses and group by status
            var taskStatusData = _context.MstrTasks
                .Where(t => t.Status != null)
                .GroupBy(t => t.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    TaskCount = g.Count()
                })
                .ToList();

            ViewBag.TaskStatusLabels = taskStatusData.Select(t => t.Status).ToList();
            ViewBag.TaskStatusValues = taskStatusData.Select(t => t.TaskCount).ToList();


            ////Fetch Customer statuses 
            //var Customerdata = _context.MstrCustomers
            //    .GroupBy(p => p.Status == true)
            //    .Select(g => new
            //    {
            //        CustomerStatus = g.Key ? "Active" : "Inactive",
            //        CustomerCount = g.Count()
            //    })
            //    .ToList();

            //ViewBag.CustomerLabels = Customerdata.Select(p => p.CustomerStatus).ToList();
            //ViewBag.CustomerValues = Customerdata.Select(p => p.CustomerCount).ToList();


            return View();

        }
        public IActionResult Administrator()
        {
            // Fetch active users grouped by role
            var roleData = _context.MstrUsers
                .Where(u => u.IsUserActivated == true)
                .GroupBy(u => u.Role.Name)
                .Select(g => new
                {
                    RoleName = g.Key,
                    UserCount = g.Count()
                })
                .ToList();

            ViewBag.Labels = roleData.Select(r => r.RoleName).ToList();
            ViewBag.Values = roleData.Select(r => r.UserCount).ToList();


            //// Fetch active and inactive projects
            //var projectData = _context.MstrProjects
            //     .Where(p => p.Status != null)
            //    .GroupBy(p => p.Status)
            //    .Select(g => new
            //    {
            //        ProjectStatus = g.Key, /*"Active" : "Inactive",*/
            //        ProjectCount = g.Count()
            //    })
            //    .ToList();

            //ViewBag.ProjectLabels = projectData.Select(p => p.ProjectStatus).ToList();
            //ViewBag.ProjectValues = projectData.Select(p => p.ProjectCount).ToList();

            //// Fetch task statuses and group by status
            //var taskStatusData = _context.MstrTasks
            //    .Where(t => t.Status != null)
            //    .GroupBy(t => t.Status)
            //    .Select(g => new
            //    {
            //        Status = g.Key,
            //        TaskCount = g.Count()
            //    })
            //    .ToList();

            //ViewBag.TaskStatusLabels = taskStatusData.Select(t => t.Status).ToList();
            //ViewBag.TaskStatusValues = taskStatusData.Select(t => t.TaskCount).ToList();


            ////Fetch Customer statuses 
            //var Customerdata = _context.MstrCustomers
            //    .GroupBy(p => p.Status == true)
            //    .Select(g => new
            //    {
            //        CustomerStatus = g.Key ? "Active" : "Inactive",
            //        CustomerCount = g.Count()
            //    })
            //    .ToList();

            //ViewBag.CustomerLabels = Customerdata.Select(p => p.CustomerStatus).ToList();
            //ViewBag.CustomerValues = Customerdata.Select(p => p.CustomerCount).ToList();


            return View();

        }
        public IActionResult Customer()
        {
            // Fetch active users grouped by role
            //var roleData = _context.MstrUsers
            //    .Where(u => u.IsUserActivated == true)
            //    .GroupBy(u => u.Role.Name)
            //    .Select(g => new
            //    {
            //        RoleName = g.Key,
            //        UserCount = g.Count()
            //    })
            //    .ToList();

            //ViewBag.Labels = roleData.Select(r => r.RoleName).ToList();
            //ViewBag.Values = roleData.Select(r => r.UserCount).ToList();


            // Fetch active and inactive projects
            var projectData = _context.MstrProjects
                 .Where(p => p.Status != null)
                .GroupBy(p => p.Status)
                .Select(g => new
                {
                    ProjectStatus = g.Key, /*"Active" : "Inactive",*/
                    ProjectCount = g.Count()
                })
                .ToList();

            ViewBag.ProjectLabels = projectData.Select(p => p.ProjectStatus).ToList();
            ViewBag.ProjectValues = projectData.Select(p => p.ProjectCount).ToList();

            // Fetch task statuses and group by status
            var taskStatusData = _context.MstrTasks
                .Where(t => t.Status != null)
                .GroupBy(t => t.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    TaskCount = g.Count()
                })
                .ToList();

            ViewBag.TaskStatusLabels = taskStatusData.Select(t => t.Status).ToList();
            ViewBag.TaskStatusValues = taskStatusData.Select(t => t.TaskCount).ToList();


            ////Fetch Customer statuses 
            //var Customerdata = _context.MstrCustomers
            //    .GroupBy(p => p.Status == true)
            //    .Select(g => new
            //    {
            //        CustomerStatus = g.Key ? "Active" : "Inactive",
            //        CustomerCount = g.Count()
            //    })
            //    .ToList();

            //ViewBag.CustomerLabels = Customerdata.Select(p => p.CustomerStatus).ToList();
            //ViewBag.CustomerValues = Customerdata.Select(p => p.CustomerCount).ToList();


            return View();

        }
    }

}

