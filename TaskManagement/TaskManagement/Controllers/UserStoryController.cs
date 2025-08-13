using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Policy;
using TaskManagement.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagement.Controllers
{
    [SessionCheck]
    public class UserStoryController : Controller
    {
        private readonly TaskManagementContext db;

        #region ctor
       
        public UserStoryController(TaskManagementContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(context));
            
        }
        #endregion

        public IActionResult UserStory()
        {
            // Fetch user stories with related entities (Project, Owner, and LastModifiedBy)
            var userStories = db.MstrUserStories
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

            // Fetch related Projects and Users to populate dropdowns or other UI elements
            var projects = db.MstrProjects
                .Select(p => new
                {
                    p.ProjectId,
                    p.ProjectTitle
                }).ToList();

            var users = db.MstrUsers
                .Select(u => new
                {
                    u.UserId,
                    u.Name
                }).ToList();

            // Pass the Projects and Users to the view
            ViewBag.Projects = projects;
            ViewBag.Users = users;

            return View();
        }

        //public IActionResult UserStory()
        //{

        //    var userstory = db.MstrUserStories

        //        .ToList();
        //    if (userstory == null || !userstory.Any())
        //    {
        //        Console.WriteLine("No userstories found in the database.");
        //    }
        //    List<MasterCustomer> customers = new List<MasterCustomer>();
        //    var query = db.MstrCustomers.ToList();
        //    foreach (var customer in query)
        //    {
        //        customers.Add(new MasterCustomer() { CustomerId = customer.CustomerId.ToString(), CustomerName = customer.CustomerName });
        //    }
        //    ViewBag.Customers = customers;

        //    List<MasterProject> projects = new List<MasterProject>();
        //    var p = db.MstrProjects.ToList();
        //    foreach (var project in p)
        //    {
        //        projects.Add(new MasterProject() { ProjectId = project.ProjectId.ToString(), ProjectTitle = project.ProjectTitle });
        //    }
        //    ViewBag.Projects = projects;

        //    List<MasterLastModifiedBy> users = new List<MasterLastModifiedBy>();
        //    var u = db.MstrUsers.ToList();
        //    foreach (var user in u)
        //    {
        //        users.Add(new MasterLastModifiedBy() { UserId = user.UserId.ToString(), Name = user.Name });
        //    }
        //    ViewBag.Users = users;

        //    List<MasterOwnerId> owners = new List<MasterOwnerId>();
        //    var o = db.MstrUsers.ToList();
        //    Guid UserRoleId = new Guid("8CE9CBA9-6BD8-4638-9227-800BB500BF7B");
        //    Guid TeamLeadRoleId = new Guid("46F44F42-4ACB-4D45-9DFA-E3BAF70C33F3");

        //    foreach (var owner in o)
        //    {
        //        if (owner.RoleId != null && (owner.RoleId == UserRoleId || owner.RoleId == TeamLeadRoleId))
        //        {

        //            owners.Add(new MasterOwnerId() { UserId = owner.UserId.ToString(), Name = owner.Name });

        //        }
        //    }
        //    ViewBag.Owners = owners;

        //    return View(userstory);
        //}

        public IActionResult Create()
        {
            List<MasterCustomer> customers = new List<MasterCustomer>();
            var query = db.MstrCustomers.ToList();
            foreach (var customer in query)
            {
                customers.Add(new MasterCustomer() { CustomerId = customer.CustomerId.ToString(), CustomerName = customer.CustomerName });
            }
            ViewBag.Customers = customers;

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
            Guid UserRoleId = new Guid("8CE9CBA9-6BD8-4638-9227-800BB500BF7B");
            Guid TeamLeadRoleId = new Guid("46F44F42-4ACB-4D45-9DFA-E3BAF70C33F3");

            foreach (var owner in o)
            {
                if (owner.RoleId != null && (owner.RoleId == UserRoleId || owner.RoleId == TeamLeadRoleId))
                {

                    owners.Add(new MasterOwnerId() { UserId = owner.UserId.ToString(), Name = owner.Name });

                }
            }
            ViewBag.Owners = owners;

            return View();
        }
        [HttpPost]
        public IActionResult Create (FileUserStory mstrUserStory)

        {
            //if (!ModelState.IsValid)
            //{
            //    return View(mstrUserStory);
            //}

            MstrUserStory userstory = new MstrUserStory();
            userstory.UserStoryId = Guid.NewGuid();
            userstory.ProjectId = new Guid(mstrUserStory.ProjectId);
            userstory.Points = mstrUserStory.Points;
            userstory.OwnerId = new Guid(mstrUserStory.OwnerId);
            userstory.Description = mstrUserStory.Description;
            //userstory.LastModifiedBy= string.IsNullOrEmpty(mstrUserStory.LastModifiedBy) ? null : new Guid(mstrUserStory.LastModifiedBy);
            //userstory.LastModifiedOn = mstrUserStory.LastModifiedOn;
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

            return RedirectToAction("UserStory");
        }

        public IActionResult Edit(string id)
        {
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



                List <Notes> pnoteslist = new List<Notes>();
                var noteslist = db.GnrlNotes.Where(c=>c.ObjectId == new Guid(id)).ToList();
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
                Guid UserRoleId = new Guid("8CE9CBA9-6BD8-4638-9227-800BB500BF7B");
                Guid TeamLeadRoleId = new Guid("46F44F42-4ACB-4D45-9DFA-E3BAF70C33F3");

                foreach (var owner in o)
                {
                    if (owner.RoleId != null && (owner.RoleId == UserRoleId || owner.RoleId == TeamLeadRoleId))
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
            // if the mode is invalid
            //if (!ModelState.IsValid)
            //{
            //    return View(mstrUserStory);
            //}


            var userstory = db.MstrUserStories.FirstOrDefault(x => x.UserStoryId == new Guid(id));
            if (userstory == null)
            {
                return NotFound();
            }
            else
            {
                //MstrUserStory userstory = new MstrUserStory();
                //userstory.UserStoryId = Guid.NewGuid();
                userstory.ProjectId = new Guid(mstrUserStory.ProjectId);
                userstory.Points = mstrUserStory.Points;
                userstory.OwnerId = new Guid(mstrUserStory.OwnerId);
                userstory.Description = mstrUserStory.Description;
                userstory.LastModifiedBy = mstrUserStory.LastModifiedBy;
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
  
            }
            return RedirectToAction("UserStory");

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


        //public IActionResult Details(Guid id)
        //{
        //    var userstory = db.MstrUserStories.Include(u => u.ProjectId).Include(u => u.LastModifiedBy).FirstOrDefault(x => x.UserStoryId == id);

        //    if (userstory == null)
        //    {
        //        return UserStoryNotFound();
        //    }
        //    return View(userstory);

        //}

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


    }
}