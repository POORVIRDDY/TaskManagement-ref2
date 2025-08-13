using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Policy;
using TaskManagement.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagement.Controllers
{
    [SessionCheck]
    public class ProjectManagerLandingController : Controller
    {
        private readonly TaskManagementContext db;
        public ProjectManagerLandingController(TaskManagementContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IActionResult Project()
        {
            var UserID = HttpContext.Session.GetString("UserID");
            var projects = db.MstrProjects
                .Where(c => c.ManagerId.ToString() == UserID)
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
            var UserID = HttpContext.Session.GetString("UserID");
            MstrProject project = new MstrProject();
            project.ProjectId = Guid.NewGuid();
            //project.SerialNumber = mstrProject.SerialNumber;

            var settings = db.GnrlSettings.FirstOrDefault();
            if (settings != null)
            {
                string prefix = settings.UserStoryPrefix;
                int counter = settings.UserStoryCounter ?? 0;
                //project.SerialNumber = prefix + " " + counter.ToString();
                settings.UserStoryCounter = settings.UserStoryCounter + 1;
                project.SerialNumber = settings.UserStoryCounter ?? 0;
                db.Entry(settings).State = EntityState.Modified;
            }


            project.ProjectTitle = mstrProject.ProjectTitle;
            project.Description = mstrProject.Description;
            project.StartDate = mstrProject.StartDate;
            project.EndDate = mstrProject.EndDate;
            project.ManagerId = new Guid(UserID);
            //project.Manager = mstrProject.Manager;
            var type=project.Type = mstrProject.Type;
            //project.Customer = mstrProject.Customer;
            if (type == "Internal")
            {
                project.CustomerId = null;
            }
            else
            {
                project.CustomerId = mstrProject.CustomerId;
            }
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
            var UserID = HttpContext.Session.GetString("UserID");
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
                //data.Type = mstrProject.Type;
                var type = data.Type = mstrProject.Type;
                if (type == "Internal")
                {
                    data.CustomerId = null;
                }
                else
                {
                    data.CustomerId = mstrProject.CustomerId;
                }
                data.Status = mstrProject.Status;

                //data.Status = mstrProject.Status;
                //data.LastModifiedOn = mstrProject.LastModifiedOn;
                data.LastModifiedOn = DateTime.Now;
                data.LastModifiedBy = new Guid(UserID);
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
            if (project == null)
            {
                return ProjectNotFound();
            }
            HttpContext.Session.SetString("ProjectID", id.ToString());
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

        //[HttpGet]
        //public IActionResult UserStory()
        //{
        //    var UserID =HttpContext.Session.GetString("UserID");
        //    MstrUserStory story = new MstrUserStory();
        //    //Guid id = new Guid("A9C4AF44-45A4-449A-8AF4-B751688D2A3A");
        //    var projects = db.MstrProjects
        //    .Where(c => c.ManagerId.ToString() == UserID)
        //    .Select(p => new
        //    {
        //        p.ProjectId,
        //        p.ProjectTitle
        //    }).ToList();
        //    ViewBag.Projects = projects;

        //    var userstories = db.MstrUserStories.Where(c => c.ProjectId == Guid.Empty).ToList();
        //    ViewBag.UserStories = userstories;
        //    return View(story);
        //}

        //[HttpPost]
        public IActionResult UserStory( Guid id)
        {

            //var UserID = HttpContext.Session.GetString("UserID");
            //var projects = db.MstrProjects
            //.Where(c => c.ManagerId.ToString() == UserID)
            //.Select(p => new
            //{
            //    p.ProjectId,
            //    p.ProjectTitle
            //}).ToList();
            //ViewBag.Projects = projects;
            
           

            var userStories = db.MstrUserStories
                .Where(c => c.ProjectId == id)
                 .Include(us => us.Project)  
                 .Include(us => us.Owner)   
                 .Include(us => us.LastModifiedByNavigation) 
                 .Select(us => new
                 {
                     us.UserStoryId,
                     us.Description,
                     us.Points,
                     us.IsNotesAttached,
                     ProjectTitle = us.Project.ProjectTitle, 
                     OwnerName = us.Owner.Name,  
                     LastModifiedByName = us.LastModifiedByNavigation.Name, 
                     us.LastModifiedOn,

                 })
                 .ToList();
            ViewBag.UserStories = userStories;
            
            return View();
        }

        public IActionResult CreateUserStory()
        {
            var projectIdString = HttpContext.Session.GetString("ProjectID");
            var projectId = new Guid(projectIdString);

            var project = db.MstrProjects.FirstOrDefault(p => p.ProjectId == projectId);
            ViewBag.Project = project;

            MstrUserStory userstory = new MstrUserStory
            {
                UserStoryId = Guid.NewGuid(),
                ProjectId = projectId,
            };

            List<MasterCustomer> customers = new List<MasterCustomer>();
            var query = db.MstrCustomers.ToList();
            foreach (var customer in query)
            {
                customers.Add(new MasterCustomer() { CustomerId = customer.CustomerId.ToString(), CustomerName = customer.CustomerName });
            }
            ViewBag.Customers = customers;

            //List<MasterProject> projects = new List<MasterProject>();
            //var p = db.MstrProjects.ToList();
            //foreach (var project in p)
            //{
            //    projects.Add(new MasterProject() { ProjectId = project.ProjectId.ToString(), ProjectTitle = project.ProjectTitle });
            //}
            //ViewBag.Projects = projects;

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

            List<MasterOwnerId> owners = new List<MasterOwnerId>();
            var o = db.MstrUsers.ToList();
            Guid TeamLeadRoleId = new Guid("46F44F42-4ACB-4D45-9DFA-E3BAF70C33F3");
            foreach (var owner in o)
            {
                if (owner.RoleId != null && owner.RoleId == TeamLeadRoleId)
                {

                    owners.Add(new MasterOwnerId() { UserId = owner.UserId.ToString(), Name = owner.Name });

                }
            }
            ViewBag.Owners = owners;

            return View();
        }
        [HttpPost]
        public IActionResult CreateUserStory(FileUserStory mstrUserStory)

        {
            //if (!ModelState.IsValid)
            //{
            //    return View(mstrUserStory);
            //}
            var projectIdString = HttpContext.Session.GetString("ProjectID");
            var projectId = new Guid(projectIdString);

            MstrUserStory userstory = new MstrUserStory();
            userstory.UserStoryId = Guid.NewGuid();
            userstory.ProjectId = projectId;
            userstory.Points = mstrUserStory.Points;
            userstory.OwnerId = new Guid(mstrUserStory.OwnerId);
            userstory.Description = mstrUserStory.Description;
            userstory.LastModifiedBy = mstrUserStory.LastModifiedBy;
            userstory.LastModifiedOn = mstrUserStory.LastModifiedOn;
            userstory.IsNotesAttached = false;

            if (mstrUserStory.File != null && mstrUserStory.File.Length > 0)
            {

                using (var memoryStream = new MemoryStream())
                {
                    //await model.File.CopyToAsync(memoryStream);
                    byte[] fileData = memoryStream.ToArray();

                    var gnrlNote = new GnrlNote
                    {
                        NotesId = Guid.NewGuid(),
                        ObjectId = userstory.UserStoryId,
                        FileName = mstrUserStory.File.FileName,
                        Data = fileData,
                        LastModifiedOn = DateTime.Now,

                    };

                    db.GnrlNotes.Add(gnrlNote);
                    //await db.SaveChangesAsync();

                    //var result = new ResultModel(model.File.FileName, model.File.Length, model.File.ContentType, MalwareStatus.Unknown, null);
                    //return View("Result", result);
                    userstory.IsNotesAttached = true;
                }
            }

            db.Entry(userstory).State = EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("UserStory", new { id = userstory.ProjectId });
        }
        public IActionResult EditUserStory(string id)
        {
            var projectIdString = HttpContext.Session.GetString("ProjectID");
            var projectId = new Guid(projectIdString);

            //var project = db.MstrProjects.FirstOrDefault(p => p.ProjectId == projectId);
            //ViewBag.Project = project;
            var projects = new SelectList(db.MstrProjects.ToList(), "ProjectId", "ProjectTitle");
            ViewBag.Projects = projects;

            var data = db.MstrUserStories.FirstOrDefault(x => x.UserStoryId == new Guid(id));
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                FileUserStory userstory = new FileUserStory();
                

                userstory.UserStoryId = data.UserStoryId;
                userstory.ProjectId = data.ProjectId.ToString();
                userstory.OwnerId = data.OwnerId.ToString();
                userstory.LastModifiedBy = data.LastModifiedBy;
                userstory.LastModifiedOn = DateTime.Now;
                userstory.Points = data.Points;
                userstory.Description = data.Description;
                userstory.IsNotesAttached = data.IsNotesAttached;

                //ViewBag.UserStories = userstory;


                List<Notes> pnoteslist = new List<Notes>();
                var noteslist = db.GnrlNotes.Where(c => c.ObjectId == new Guid(id)).ToList();
                foreach (var pnote in noteslist)
                {
                    Notes note = new Notes();
                    note.NotesId = pnote.NotesId.ToString();
                    note.FileName = pnote.FileName.ToString();
                    pnoteslist.Add(note);
                }
                userstory.Notes = pnoteslist;

                List<MasterCustomer> customers = new List<MasterCustomer>();
                var query = db.MstrCustomers.ToList();
                foreach (var customer in query)
                {
                    customers.Add(new MasterCustomer() { CustomerId = customer.CustomerId.ToString(), CustomerName = customer.CustomerName });
                }
                ViewBag.Customers = customers;

                //List<MasterProject> projects = new List<MasterProject>();
                //var p = db.MstrProjects.ToList();
                //foreach (var project in p)
                //{
                //    projects.Add(new MasterProject() { ProjectId = project.ProjectId.ToString(), ProjectTitle = project.ProjectTitle });
                //}
                //ViewBag.Projects = projects;

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

                List<MasterOwnerId> owners = new List<MasterOwnerId>();
                var o = db.MstrUsers.ToList();
                Guid TeamLeadRoleId = new Guid("46F44F42-4ACB-4D45-9DFA-E3BAF70C33F3");
                foreach (var owner in o)
                {
                    if (owner.RoleId != null && owner.RoleId == TeamLeadRoleId)
                    {
                        owners.Add(new MasterOwnerId() { UserId = owner.UserId.ToString(), Name = owner.Name });
                    }
                }
                ViewBag.Owners = owners;

                return View(userstory);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUserStory(System.String id, FileUserStory mstrUserStory)
        {
            // if the mode is invalid
            //if (!ModelState.IsValid)
            //{
            //    return View(mstrUserStory);
            //}
            var UserID = HttpContext.Session.GetString("UserID");

            var projectIdString = HttpContext.Session.GetString("ProjectID");
            var projectId = new Guid(projectIdString);

            var userstory = db.MstrUserStories.FirstOrDefault(x => x.UserStoryId == new Guid(id));
            if (userstory == null)
            {
                return NotFound();
            }
            else
            {
                //MstrUserStory userstory = new MstrUserStory();
                //userstory.UserStoryId = Guid.NewGuid();
                userstory.ProjectId = projectId;
                userstory.Points = mstrUserStory.Points;
                userstory.OwnerId = new Guid(mstrUserStory.OwnerId);
                userstory.Description = mstrUserStory.Description;
                userstory.LastModifiedBy = new Guid(UserID);
                userstory.LastModifiedOn = DateTime.Now;
                //userstory.IsNotesAttached = mstrUserStory.IsNotesAttached;


                if (mstrUserStory.File != null && mstrUserStory.File.Length > 0)
                {
                    var fileName = mstrUserStory.File.FileName;

                    using (var memoryStream = new MemoryStream())
                    {
                        mstrUserStory.File.CopyTo(memoryStream);
                        //await model.File.CopyToAsync(memoryStream);
                        byte[] fileData = memoryStream.ToArray();

                        var gnrlNote = new GnrlNote
                        {
                            NotesId = Guid.NewGuid(),
                            ObjectId = new Guid(id),
                            FileName = mstrUserStory.File.FileName,
                            Data = fileData,
                            LastModifiedOn = DateTime.Now,

                        };

                        db.GnrlNotes.Add(gnrlNote);
                        //await db.SaveChangesAsync();

                        //var result = new ResultModel(model.File.FileName, model.File.Length, model.File.ContentType, MalwareStatus.Unknown, null);
                        //return View("Result", result);


                        if (userstory.IsNotesAttached != true)
                        {
                            userstory.IsNotesAttached = true;
                        }
                    }
                    ViewBag.FileName = fileName;

                }
                db.Entry(userstory).State = EntityState.Modified;
                db.SaveChanges();

                //ViewBag.UserStories = userstory;

            }

            //return View(userstory);
            return RedirectToAction("UserStory",new {id= userstory.ProjectId });

        }
        public IActionResult DownloadFile(Guid noteId)
        {

            var note = db.GnrlNotes.FirstOrDefault(n => n.NotesId == noteId);

            if (note == null)
            {
                return NotFound();
            }
            return File(note.Data, "application/octet-stream", note.FileName);
        }
        public IActionResult DetailsUserStory(Guid id)
        {
            var userstory = db.MstrUserStories
                .Include(u => u.Project)
                .Include(p => p.LastModifiedByNavigation)
                .Include(p => p.Owner)
                .FirstOrDefault(x => x.UserStoryId == id);

            if (userstory == null)
            {
                return UserStoryNotFound();
            }

            return View(userstory);
        }
        public IActionResult UserStoryNotFound()
        {
            return View();
        }

        //[HttpGet]
        //public IActionResult Task()
        //{
        //    var UserID = HttpContext.Session.GetString("UserID");
        //    MstrTask task1 = new MstrTask();
        //    var projects = db.MstrProjects
        //    .Where(c => c.ManagerId.ToString() == UserID)
        //    .Select(p => new
        //    {
        //        p.ProjectId,
        //        p.ProjectTitle
        //    }).ToList();
        //    ViewBag.Projects = projects;

        //    var tasks = db.MstrTasks.Where(c => c.ProjectId == Guid.Empty).ToList();
        //    ViewBag.Tasks = tasks;
        //    return View(task1);

        //}

        //[HttpPost]
        public IActionResult Task(MstrTask task1, Guid id)
        {

            //var UserID = HttpContext.Session.GetString("UserID");
            //var projects = db.MstrProjects
            //.Where(c => c.ManagerId.ToString() == UserID)
            //.Select(p => new
            //{
            //    p.ProjectId,
            //    p.ProjectTitle
            //}).ToList();
            //ViewBag.Projects = projects;

            var tasks = db.MstrTasks
                .Where(c => c.UserStoryId == id)
                .Include(t => t.AssignedToNavigation)  // Assigned user
                .Include(t => t.Project)               // Project
                .Include(t => t.UserStory)             // User Story
                .Select(t => new
                {
                    t.TaskId,
                    t.TaskNo,
                    AssignedTo = t.AssignedToNavigation.Name,  // Assuming AssignedTo is a User entity with Name
                    t.Description,
                    t.Status,
                    ProjectTitle = t.Project.ProjectTitle,  // Assuming Project has ProjectTitle
                    UserStory = t.UserStory.Description,  // Assuming UserStory has Description
                    t.StartDate,
                    t.EndDate,
                    t.TotalEstimatedHours,
                    t.TotalHoursSpent,
                    t.TotalRemainingHours,
                    t.IsNotesAttached,
                })
                .ToList();

            ViewBag.Tasks = tasks;  // Pass the task data to the view
            return View();
        }
        public IActionResult EditTask(string id)
        {
            var task = db.MstrTasks.FirstOrDefault(x => x.TaskId == new Guid(id));
            if (task == null)
            {
                return NotFound();
            }
            else
            {
                FileTask data = new FileTask();

                data.TaskNo = task.TaskNo;
                data.AssignedTo = task.AssignedTo;
                data.ProjectId = task.ProjectId;
                data.Description = task.Description;
                data.Status = task.Status;
                data.UserStoryId = task.UserStoryId;
                data.ParentTaskId = task.ParentTaskId;
                data.StartDate = task.StartDate;
                data.EndDate = task.EndDate;
                data.TotalEstimatedHours = task.TotalEstimatedHours;
                data.TotalHoursSpent = task.TotalHoursSpent;
                data.TotalRemainingHours = task.TotalRemainingHours;
                data.IsNotesAttached = task.IsNotesAttached;

                List<Notes> pnoteslist = new List<Notes>();
                var noteslist = db.GnrlNotes.Where(c => c.ObjectId == new Guid(id)).ToList();
                foreach (var pnote in noteslist)
                {
                    Notes note = new Notes();
                    note.NotesId = pnote.NotesId.ToString();
                    note.FileName = pnote.FileName.ToString();
                    pnoteslist.Add(note);
                }
                data.Notes = pnoteslist;

                List<MasterProject> projects = new List<MasterProject>();
                var p = db.MstrProjects.ToList();
                foreach (var project in p)
                {
                    projects.Add(new MasterProject() { ProjectId = project.ProjectId.ToString(), ProjectTitle = project.ProjectTitle });
                }
                ViewBag.Projects = projects;

                List<MasterAssignedTo> users = new List<MasterAssignedTo>();
                var u = db.MstrUsers.ToList();
                Guid UserRoleId = new Guid("8CE9CBA9-6BD8-4638-9227-800BB500BF7B");
                Guid TeamLeadRoleId = new Guid("46F44F42-4ACB-4D45-9DFA-E3BAF70C33F3");

                foreach (var user in u)
                {
                    if (user.RoleId != null && (user.RoleId == UserRoleId || user.RoleId == TeamLeadRoleId))
                    {

                        users.Add(new MasterAssignedTo() { UserId = user.UserId.ToString(), Name = user.Name });


                    }
                }
                ViewBag.Users = users;

                List<MasterUserStory> userstories = new List<MasterUserStory>();
                var s = db.MstrUserStories.ToList();
                foreach (var userStory in s)
                {
                    userstories.Add(new MasterUserStory() { UserStoryId = userStory.UserStoryId.ToString(), Description = userStory.Description });
                }
                ViewBag.Userstory = userstories;

                List<MasterTask> tasks = new List<MasterTask>();
                var t = db.MstrTasks.ToList();
                foreach (var parenttask in t)
                {
                    tasks.Add(new MasterTask() { TaskId = parenttask.TaskId.ToString(), TaskNo = parenttask.TaskNo });
                }
                ViewBag.Tasks = tasks;

                List<MasterProject> projects1 = new List<MasterProject>();
                var p1 = db.MstrProjects.ToList();
                foreach (var project in p1)
                {
                    projects1.Add(new MasterProject() { ProjectId = project.ProjectId.ToString(), ProjectTitle = project.ProjectTitle });
                }
                ViewBag.Projects = projects1;

                return View(data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTask(System.String id, FileTask mstrTask)
        {

            var task = db.MstrTasks.FirstOrDefault(x => x.TaskId == new Guid(id));
            if (task == null)
            {
                return NotFound();
            }
            else
            {


                task.TaskNo = mstrTask.TaskNo;
                task.AssignedTo = mstrTask.AssignedTo;
                task.ProjectId = mstrTask.ProjectId;
                task.Description = mstrTask.Description;
                task.Status = mstrTask.Status.ToString();
                task.UserStoryId = mstrTask.UserStoryId;
                task.ParentTaskId = mstrTask.ParentTaskId;
                task.StartDate = mstrTask.StartDate;
                task.EndDate = mstrTask.EndDate;
                task.TotalEstimatedHours = mstrTask.TotalEstimatedHours;
                task.TotalHoursSpent = mstrTask.TotalHoursSpent;
                task.TotalRemainingHours = mstrTask.TotalRemainingHours;
                //task.IsNotesAttached = mstrTask.IsNotesAttached;

                if (mstrTask.File != null && mstrTask.File.Length > 0)
                {
                    var fileName = mstrTask.File.FileName;

                    using (var memoryStream = new MemoryStream())
                    {

                        byte[] fileData = memoryStream.ToArray();

                        var gnrlNote = new GnrlNote
                        {
                            NotesId = Guid.NewGuid(),
                            ObjectId = task.TaskId,
                            FileName = mstrTask.File.FileName,
                            Data = fileData,
                            LastModifiedOn = DateTime.Now,

                        };

                        db.GnrlNotes.Add(gnrlNote);
                        if (task.IsNotesAttached != true)
                        {
                            task.IsNotesAttached = true;
                        }
                    }
                    ViewBag.FileName = fileName;

                }

                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();

            }
            return RedirectToAction("Task", new{id=task.UserStoryId});
        }
        public IActionResult DownloadFileTask(Guid noteId)
        {

            var note = db.GnrlNotes.FirstOrDefault(n => n.NotesId == noteId);

            if (note == null)
            {
                return NotFound();
            }
            return File(note.Data, "application/octet-stream", note.FileName);
        }
        public IActionResult DetailsTask(Guid id)
        {
            var task = db.MstrTasks
                    .Include(t => t.Project)
                    .Include(t => t.UserStory)
                    .Include(t => t.ParentTask)
                    .Include(t => t.AssignedToNavigation)
                    .FirstOrDefault(t => t.TaskId == id);

            if (task == null)
            {
                return TaskNotFound();
            }
            return View(task);


        }
        public IActionResult TaskNotFound()
        {
            return View();
        }
    }
}
