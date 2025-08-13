using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using TaskManagement.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagement.Controllers
{
    [SessionCheck]
    public class TeamLeadLandingController : Controller
    {


        private readonly TaskManagementContext db;
        public TeamLeadLandingController(TaskManagementContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(context));

        }

        [HttpGet]
        public IActionResult UserStory()
        {
            var UserID = HttpContext.Session.GetString("UserID");
            MstrUserStory story = new MstrUserStory();

            var Tprojects = db.MstrUserStories
                .Where(c => c.OwnerId.ToString() == UserID)
            .Select(p => new
            {
                p.ProjectId,
                ProjectTitle = p.Project.ProjectTitle,

            }).Distinct()
            .ToList();
            ViewBag.TProjects = Tprojects;

            var userstories = db.MstrUserStories
                .Where(c => c.ProjectId == Guid.Empty)
                .Where(c => c.OwnerId.ToString() == UserID)
                .ToList();
            ViewBag.UserStories = userstories;
            return View(story);
        }
        [HttpPost]
        public IActionResult UserStory(MstrUserStory story, Guid id)
        {

            var UserID = HttpContext.Session.GetString("UserID");
            var Tprojects = db.MstrUserStories
               .Where(c => c.OwnerId.ToString() == UserID)
           .Select(p => new
           {
               p.ProjectId,
               ProjectTitle = p.Project.ProjectTitle,

           }).Distinct()
           .ToList();
            ViewBag.TProjects = Tprojects;

            HttpContext.Session.SetString("ProjectID", story.ProjectId.ToString());


            var userStories = db.MstrUserStories
                .Where(c => c.ProjectId == story.ProjectId)
                .Where(c => c.OwnerId.ToString() == UserID)
                 .Include(us => us.Project)  // Include the related Project
                 .Include(us => us.Owner)    // Include the related Owner
                 .Include(us => us.LastModifiedByNavigation) // Include the LastModifiedByNavigation (modifier)
                 .Select(us => new
                 {
                     us.UserStoryId,
                     us.Description,
                     us.Points,
                     us.IsNotesAttached,
                     ProjectTitle = us.Project.ProjectTitle,  // Assuming Project has a ProjectTitle property
                     OwnerName = us.Owner.Name,  // Assuming Owner has a Name property
                     LastModifiedByName = us.LastModifiedByNavigation.Name,  // Name of the modifier
                     us.LastModifiedOn,

                 })
                 .ToList();

            // Pass the user story data to the view via ViewBag
            ViewBag.UserStories = userStories;

            return View();
        }

        public IActionResult Create()
        {
            var projectIdString = HttpContext.Session.GetString("ProjectID");
            var projectId = new Guid(projectIdString);
            var UserIdString = HttpContext.Session.GetString("UserID");
            var userId = new Guid(UserIdString);
            MstrUserStory userstory = new MstrUserStory
            {
                UserStoryId = Guid.NewGuid(),
                ProjectId = projectId,

                OwnerId = userId,
            };
            List<MasterCustomer> customers = new List<MasterCustomer>();
            var query = db.MstrCustomers.ToList();
            foreach (var customer in query)
            {
                customers.Add(new MasterCustomer() { CustomerId = customer.CustomerId.ToString(), CustomerName = customer.CustomerName });
            }
            ViewBag.Customers = customers;

            var project = db.MstrProjects.FirstOrDefault(p => p.ProjectId == projectId);
            var owner = db.MstrUsers.FirstOrDefault(u => u.UserId == userId);
            ViewBag.Project = project;
            ViewBag.Owner = owner;

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

            //List<MasterOwnerId> owners = new List<MasterOwnerId>();
            //var o = db.MstrUsers.ToList();
            //Guid TeamLeadRoleId = new Guid("46F44F42-4ACB-4D45-9DFA-E3BAF70C33F3");

            //foreach (var owner in o)
            //{
            //    if (owner.RoleId != null && owner.RoleId == TeamLeadRoleId)
            //    {

            //        owners.Add(new MasterOwnerId() { UserId = owner.UserId.ToString(), Name = owner.Name });

            //    }
            //}
            //ViewBag.Owners = owners;

            return View();
        }
        [HttpPost]
        public IActionResult Create(FileUserStory mstrUserStory)

        {
            var projectIdString = HttpContext.Session.GetString("ProjectID");
            var projectId = new Guid(projectIdString);
            var UserIdString = HttpContext.Session.GetString("UserID");
            var userId = new Guid(UserIdString);

            //MstrUserStory userstory = new MstrUserStory();
            //userstory.UserStoryId = Guid.NewGuid();
            //userstory.ProjectId = new Guid(mstrUserStory.ProjectId);
            //userstory.Points = mstrUserStory.Points;
            //userstory.OwnerId = new Guid(mstrUserStory.OwnerId);
            //userstory.Description = mstrUserStory.Description;
            //userstory.LastModifiedBy = string.IsNullOrEmpty(mstrUserStory.LastModifiedBy) ? null : new Guid(mstrUserStory.LastModifiedBy);
            //userstory.LastModifiedOn = mstrUserStory.LastModifiedOn;
            //userstory.IsNotesAttached = false;

            MstrUserStory userstory = new MstrUserStory
            {
                UserStoryId = Guid.NewGuid(),
                ProjectId = projectId,
                Points = mstrUserStory.Points,
                OwnerId = userId,
                Description = mstrUserStory.Description,
                LastModifiedBy = mstrUserStory.LastModifiedBy,
                LastModifiedOn = mstrUserStory.LastModifiedOn,
                IsNotesAttached = false
            };
            if (mstrUserStory.File != null && mstrUserStory.File.Length > 0)
            {

                using (var memoryStream = new MemoryStream())
                {

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

                    userstory.IsNotesAttached = true;
                }
            }

            db.Entry(userstory).State = EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("UserStory");
        }
        public IActionResult Edit(string id)
        {
            var projectIdString = HttpContext.Session.GetString("ProjectID");
            var projectId = new Guid(projectIdString);
            var UserID = HttpContext.Session.GetString("UserID");
            var userId = new Guid(UserID);

            var data = db.MstrUserStories.FirstOrDefault(x => x.UserStoryId == new Guid(id));
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                FileUserStory userstory = new FileUserStory();


                userstory.UserStoryId = data.UserStoryId;
                userstory.ProjectId = projectIdString;
                userstory.OwnerId = userId.ToString();
                userstory.LastModifiedBy = data.LastModifiedBy;
                userstory.LastModifiedOn = DateTime.Now;
                userstory.Points = data.Points;
                userstory.Description = data.Description;
                userstory.IsNotesAttached = data.IsNotesAttached;



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

                //var projects = new SelectList(db.MstrProjects.ToList(), "ProjectId", "ProjectTitle");
                //ViewBag.Projects = projects;
                //var owners = new SelectList(db.MstrUsers.ToList(), "UserId", "Name");
                //ViewBag.Owners = owners;

                List<MasterProject> projects = new List<MasterProject>();
                var p = db.MstrProjects.ToList();
                foreach (var project in p)
                {
                    projects.Add(new MasterProject() { ProjectId = project.ProjectId.ToString(), ProjectTitle = project.ProjectTitle });
                }
                ViewBag.Projects = projects;

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
        public IActionResult Edit(System.String id, FileUserStory mstrUserStory)
        {
            var UserID = HttpContext.Session.GetString("UserID");
            var userId = new Guid(UserID);
            var projectIdString = HttpContext.Session.GetString("ProjectID");
            var projectId = new Guid(projectIdString);

            var userstory = db.MstrUserStories.FirstOrDefault(x => x.UserStoryId == new Guid(id));
            if (userstory == null)
            {
                return NotFound();
            }
            else
            {

                userstory.ProjectId = projectId;
                userstory.Points = mstrUserStory.Points;
                userstory.OwnerId = userId;
                userstory.Description = mstrUserStory.Description;
                userstory.LastModifiedBy = new Guid(UserID);
                
                userstory.LastModifiedOn = DateTime.Now;



                if (mstrUserStory.File != null && mstrUserStory.File.Length > 0)
                {
                    var fileName = mstrUserStory.File.FileName;

                    using (var memoryStream = new MemoryStream())
                    {
                        mstrUserStory.File.CopyTo(memoryStream);

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


                        if (userstory.IsNotesAttached != true)
                        {
                            userstory.IsNotesAttached = true;
                        }
                    }
                    ViewBag.FileName = fileName;

                }
                db.Entry(userstory).State = EntityState.Modified;
                db.SaveChanges();

            }
            return RedirectToAction("UserStory", new { id = userstory.ProjectId });

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
        public IActionResult Details(Guid id)
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
        public IActionResult Task(MstrTask task, Guid id)
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
                    ProjectTitle = t.Project.ProjectTitle,
                    t.UserStoryId,
                    UserStory = t.UserStory.Description,  // Assuming UserStory has Description
                    t.StartDate,
                    t.EndDate,
                    t.TotalEstimatedHours,
                    t.TotalHoursSpent,
                    t.TotalRemainingHours,
                    t.IsNotesAttached,
                })
                .ToList();
            HttpContext.Session.SetString("UserStoryID", id.ToString());
            ViewBag.Tasks = tasks;  // Pass the task data to the view
            return View();
        }
        public IActionResult CreateTask()
        {
            var projectIdString = HttpContext.Session.GetString("ProjectID");
            var projectId = new Guid(projectIdString);
            var UserStoryIDString = HttpContext.Session.GetString("UserStoryID");
            var userstoryId = new Guid(UserStoryIDString);

            //List<MasterProject> projects = new List<MasterProject>();
            //var p = db.MstrProjects.ToList();
            //foreach (var project in p)
            //{
            //    projects.Add(new MasterProject() { ProjectId = project.ProjectId.ToString(), ProjectTitle = project.ProjectTitle });
            //}
            //ViewBag.Projects = projects;

            MstrTask task = new MstrTask
            {

                TaskId = Guid.NewGuid(),
                ProjectId = projectId,
                //UserStoryId = userstoryId,
            };

            var project = db.MstrProjects.FirstOrDefault(p => p.ProjectId == projectId);
            var userstory = db.MstrUserStories.FirstOrDefault(u => u.UserStoryId == userstoryId);
            ViewBag.Project = project;
            ViewBag.Userstory = userstory;


            List<MasterAssignedTo> users = new List<MasterAssignedTo>();
            var u = db.MstrUsers.ToList();
            Guid UserRoleId = new Guid("8CE9CBA9-6BD8-4638-9227-800BB500BF7B");
          

            foreach (var user in u)
            {
                if (user.RoleId != null && user.RoleId == UserRoleId)
                {

                    users.Add(new MasterAssignedTo() { UserId = user.UserId.ToString(), Name = user.Name });


                }
            }
            ViewBag.Users = users;


            //List<MasterUserStory> userstories = new List<MasterUserStory>();
            //var s = db.MstrUserStories.ToList();
            //foreach (var userStory in s)
            //{
            //    userstories.Add(new MasterUserStory() { UserStoryId = userStory.UserStoryId.ToString(), Description = userStory.Description });
            //}
            //ViewBag.Userstory = userstories;

            List<MasterTask> tasks = new List<MasterTask>();
            var t = db.MstrTasks.Where(task => task.UserStoryId == userstoryId && task.ProjectId == projectId).ToList();
            foreach (var parenttask in t)
            {
                tasks.Add(new MasterTask() { TaskId = parenttask.TaskId.ToString(), TaskNo = parenttask.TaskNo });
            }
            ViewBag.Tasks = tasks;

            return View();

        }
        [HttpPost]
        public IActionResult CreateTask(FileTask mstrTask)

        {

            var projectIdString = HttpContext.Session.GetString("ProjectID");
            var projectId = new Guid(projectIdString);
            var UserStoryIDString = HttpContext.Session.GetString("UserStoryID");
            var userstoryId = new Guid(UserStoryIDString);

            MstrTask task = new MstrTask
            {
                TaskId = Guid.NewGuid(),  // Generate a new Task ID
                /* TaskNo = mstrTask.TaskNo, */ // Task number from the model

               

            AssignedTo = mstrTask.AssignedTo,
                ProjectId = projectId,
                Description = mstrTask.Description,
                Status = "NotStarted",
                UserStoryId = userstoryId,
                ParentTaskId = mstrTask.ParentTaskId,
                StartDate = mstrTask.StartDate,
                EndDate = mstrTask.EndDate,
                TotalEstimatedHours = mstrTask.TotalEstimatedHours,
                TotalHoursSpent = 0,
                TotalRemainingHours = 0,
                IsNotesAttached = false
            };

                var settings = db.GnrlSettings.FirstOrDefault();
            if (settings != null)
            {
                string prefix = settings.TaskPrefix;
                settings.TaskCounter = settings.TaskCounter + 1;
                task.TaskNo = prefix + " " + settings.TaskCounter.ToString();
                db.Entry(settings).State = EntityState.Modified;
            }
            //MstrTask task = new MstrTask();
            //task.TaskId = Guid.NewGuid();
            //task.TaskNo = mstrTask.TaskNo;

            ////var settings = db.GnrlSettings.FirstOrDefault();
            ////if (settings != null)
            ////{
            ////    string prefix = settings.TaskPrefix;
            ////    settings.TaskCounter = settings.TaskCounter + 1;
            ////    task.TaskNo = prefix + " " + settings.TaskCounter.ToString();
            ////    db.Entry(settings).State = EntityState.Modified;
            ////}



            //task.AssignedTo = mstrTask.AssignedTo;
            //task.ProjectId = mstrTask.ProjectId;
            //task.Description = mstrTask.Description;
            //task.Status = mstrTask.Status.ToString();
            //task.UserStoryId = mstrTask.UserStoryId;
            //task.ParentTaskId = mstrTask.ParentTaskId;
            //task.StartDate = mstrTask.StartDate;
            //task.EndDate = mstrTask.EndDate;
            //task.TotalEstimatedHours = mstrTask.TotalEstimatedHours;
            //task.TotalHoursSpent = mstrTask.TotalHoursSpent;
            //task.TotalRemainingHours = mstrTask.TotalRemainingHours;
            //task.IsNotesAttached = false;

            if (mstrTask.File != null && mstrTask.File.Length > 0)
            {

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

                    task.IsNotesAttached = true;
                }
            }


            db.Entry(task).State = EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Task", new { id = task.UserStoryId });
        }
        public IActionResult EditTask(string id)
        {
            var projectIdString = HttpContext.Session.GetString("ProjectID");
            var projectId = new Guid(projectIdString);
            var UserStoryIDString = HttpContext.Session.GetString("UserStoryID");
            var userstoryId = new Guid(UserStoryIDString);
            var task = db.MstrTasks.FirstOrDefault(x => x.TaskId == new Guid(id));
            if (task == null)
            {
                return NotFound();
            }
            else
            {
                FileTask data = new FileTask();
                data.Notes = new List<Notes>();
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
                var t = db.MstrTasks.Where(task => task.UserStoryId == userstoryId && task.ProjectId == projectId).ToList();
                foreach (var parenttask in t)
                {
                    tasks.Add(new MasterTask() { TaskId = parenttask.TaskId.ToString(), TaskNo = parenttask.TaskNo });
                }
                ViewBag.Tasks = tasks;

                return View();

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
            //var task = db.MstrTasks.Include("Project").FirstOrDefault(x => x.TaskId == new Guid(id));

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

                //#region timesheet code
                //string loginUserId = HttpContext.Session.GetString("UserID");
                //if (!string.IsNullOrEmpty(loginUserId))
                //{
                //    task.TotalEstimatedHours = mstrTask.TotalEstimatedHours;
                //    if (task.TotalHoursSpent == mstrTask.TotalHoursSpent)
                //    {
                //    }
                //    else
                //    {
                //        // here we need to insert record in MstrTimesheet
                //        MstrTimeSheet timesheet = new MstrTimeSheet();
                //        timesheet.TimeSheetId = Guid.NewGuid();
                //        timesheet.UserId = new Guid(loginUserId);
                //        timesheet.TaskId = new Guid(id);
                //        timesheet.ProjectId = mstrTask.ProjectId;
                //        timesheet.Date = DateTime.Today;
                //        timesheet.Status = true;
                //        timesheet.Hours = mstrTask.TotalHoursSpent - task.TotalHoursSpent;
                //        timesheet.ApprovedBy = task.Project.ManagerId;
                //        timesheet.AddedOn = DateTime.Today;
                //        db.MstrTimeSheets.Add(timesheet);
                //        db.SaveChanges();
                //    }
                //}
                //#endregion

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
            return RedirectToAction("Task", new { id = task.UserStoryId });
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
