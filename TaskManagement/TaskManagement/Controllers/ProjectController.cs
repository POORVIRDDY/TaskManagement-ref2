using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagement.Models;
using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagement.Controllers
{
    [SessionCheck]
    public class ProjectController : Controller
    {
        private readonly TaskManagementContext db;

        public ProjectController(TaskManagementContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(context));
        }


        public IActionResult Project()
        {
          
            var projects = db.MstrProjects
                .Include(p => p.Customer)
                    .Include(p => p.Manager)
                    .Include(p => p.LastModifiedByNavigation) 
                .Select(p => new
                {
                    p.ProjectId,
                    p.ProjectTitle,
                    p.Customer.CustomerName, 
                    p.Manager.Name,    
                    lastmodifiedby = p.LastModifiedByNavigation.Name,
                    p.LastModifiedOn,
                    p.Type,
                    p.StartDate,
                    p.EndDate,
                    p.Status
                })
                .ToList();

            ViewBag.Projects = projects;  
            return View();
        }

        public IActionResult Create()
        {
            List<MasterCustomer> customers = new List<MasterCustomer>();
            var query = db.MstrCustomers.ToList();
            foreach (var customer in query)
            {
                customers.Add(new MasterCustomer() { CustomerId = customer.CustomerId.ToString(), CustomerName = customer.CustomerName });
            }
            ViewBag.Customers = customers;

            List<MasterManager> managers = new List<MasterManager>();
            var m = db.MstrUsers.ToList();
            const string ManagerRoleId = "726ef062-88da-4d2d-9791-50239dcb2142";
            foreach (var manager in m)
            {
                if (manager.RoleId.ToString() == ManagerRoleId)
                {
                    managers.Add(new MasterManager()
                    {
                        UserId = manager.UserId.ToString(),  
                        Name = manager.Name 
                    });
                }
            }
            ViewBag.Managers = managers;

            List<MasterLastModifiedBy> modifiers = new List<MasterLastModifiedBy>();
            var l = db.MstrUsers.ToList();

            const string ModifierRoleId = "726ef062-88da-4d2d-9791-50239dcb2142";

            foreach (var modifier in l)
            {
                if (modifier.RoleId.ToString() == ModifierRoleId)
                {
                    modifiers.Add(new MasterLastModifiedBy() { UserId = modifier.UserId.ToString(), Name = modifier.Name });
                }
            }
            ViewBag.Modifiers = modifiers;

            return View();
        }


        [HttpPost]
        public IActionResult Create(MstrProject mstrProject)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(mstrProject);
            //}

            MstrProject project = new MstrProject();
            project.ProjectId = Guid.NewGuid();
            project.SerialNumber = mstrProject.SerialNumber;
            project.ProjectTitle = mstrProject.ProjectTitle;
            project.Description = mstrProject.Description;
            project.StartDate = mstrProject.StartDate;
            project.EndDate = mstrProject.EndDate;
            project.ManagerId = mstrProject.ManagerId;
            //project.Manager = mstrProject.Manager;
            project.Type = mstrProject.Type;
            //project.Customer = mstrProject.Customer;
            project.CustomerId = mstrProject.CustomerId;
            project.Status = mstrProject.Status;

            
            db.Entry(project).State = EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Project");
        }


        public IActionResult Edit(string id)
        {
            var data = db.MstrProjects.FirstOrDefault(x => x.ProjectId == new Guid(id));
            if (data == null)
            {
                return NotFound();
            }
            List<MasterCustomer> customers = new List<MasterCustomer>();
            var query = db.MstrCustomers.ToList();
            foreach (var customer in query)
            {
                customers.Add(new MasterCustomer() { CustomerId = customer.CustomerId.ToString(), CustomerName = customer.CustomerName });
            }
            ViewBag.Customers = customers;

            List<MasterManager> managers = new List<MasterManager>();
            var m = db.MstrUsers.ToList();
            const string ManagerRoleId = "726ef062-88da-4d2d-9791-50239dcb2142";
            foreach (var manager in m)
            {
                if (manager.RoleId.ToString() == ManagerRoleId)
                {
                    managers.Add(new MasterManager()
                    {
                        UserId = manager.UserId.ToString(),
                        Name = manager.Name
                    });
                }
            }
            ViewBag.Managers = managers;

            //List<MasterLastModifiedBy> modifiers = new List<MasterLastModifiedBy>();
            //var l = db.MstrUsers.ToList();

            //const string ModifierRoleId = "726ef062-88da-4d2d-9791-50239dcb2142";

            //foreach (var modifier in l)
            //{
            //    if (modifier.RoleId.ToString() == ModifierRoleId)
            //    {
            //        modifiers.Add(new MasterLastModifiedBy() { UserId = modifier.UserId.ToString(), Name = modifier.Name });
            //    }
            //}
            //ViewBag.Modifiers = modifiers;
            return View(data);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, MstrProject mstrProject)
        {
            // if the mode is invalid
            //if (!ModelState.IsValid)
            //{
            //    return View(mstrProject);
            //}

            var data = db.MstrProjects.FirstOrDefault(x => x.ProjectId == id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                //MstrProject project = new MstrProject();
                data.ProjectTitle = mstrProject.ProjectTitle;
                data.Description = mstrProject.Description;
                data.StartDate = mstrProject.StartDate;
                data.EndDate = mstrProject.EndDate;
                //project.ManagerId = mstrProject.ManagerId;
                data.ManagerId = mstrProject.ManagerId;
                //project.CustomerId = mstrProject.CustomerId;
                data.Type = mstrProject.Type;
                data.CustomerId = mstrProject.CustomerId;
                data.Status = mstrProject.Status;
                //data.LastModifiedOn = mstrProject.LastModifiedOn;
                data.LastModifiedOn = DateTime.Now;
                data.LastModifiedBy = mstrProject.LastModifiedBy;
                db.Entry(data).State = EntityState.Modified;

                db.SaveChanges();

                //return RedirectToAction("Project");
            }
            return RedirectToAction("Project");

        }


        public IActionResult Details(Guid id)
        {
            var project = db.MstrProjects
                    .Include(p => p.Customer) 
                    .Include(p => p.Manager)
                    .Include(p => p.LastModifiedByNavigation)
                    .FirstOrDefault(p => p.ProjectId == id);
            //var data = db.MstrProjects.FirstOrDefault(x => x.ProjectId == id);
            if (project== null)
            {
                return ProjectNotFound();
            }
            return View(project);
        }
        public IActionResult ProjectNotFound()
        {
            return View();
        }

        public IActionResult Delete(Guid id)
        {
            var data = db.MstrProjects.FirstOrDefault(x => x.ProjectId == id);
            if (data != null)
            {
                data.Status = "Inactive";

                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Project");
        }

    }
}