using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    [SessionCheck]
    public class UserLandingController : Controller
    {
        private readonly TaskManagementContext db;

        public UserLandingController(TaskManagementContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IActionResult Task()
        {
            var UserID = HttpContext.Session.GetString("UserID");

            var tasks = db.MstrTasks
                    .Where(c => c.AssignedTo.ToString() == UserID)
                    .Include(t => t.AssignedToNavigation)
                    .Include(t => t.Project)
                    .Include(t => t.UserStory)
                    .Select(t => new
                    {
                        t.TaskId,
                        t.TaskNo,
                        AssignedTo = t.AssignedToNavigation.Name,
                        t.Description,
                        t.Status,
                        ProjectTitle = t.Project.ProjectTitle,
                        UserStory = t.UserStory.Description,
                        t.StartDate,
                        t.EndDate,
                        t.TotalEstimatedHours,
                        t.TotalHoursSpent,
                        t.TotalRemainingHours,
                        t.IsNotesAttached,
                    })
                    .ToList();

            ViewBag.Tasks = tasks;
            return View();
        }

        //public IActionResult Edit(string id)
        //{
        //    var task = db.MstrTasks.FirstOrDefault(x => x.TaskId == new Guid(id));
        //    if (task == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        FileTask data = new FileTask();

        //        data.TaskNo = task.TaskNo;
        //        data.AssignedTo = task.AssignedTo;
        //        data.ProjectId = task.ProjectId;
        //        data.Description = task.Description;
        //        data.Status = task.Status;
        //        data.UserStoryId = task.UserStoryId;
        //        data.ParentTaskId = task.ParentTaskId;
        //        data.StartDate = task.StartDate;
        //        data.EndDate = task.EndDate;
        //        data.TotalEstimatedHours = task.TotalEstimatedHours;
        //        data.TotalHoursSpent = task.TotalHoursSpent;
        //        data.TotalRemainingHours = task.TotalRemainingHours;
        //        data.IsNotesAttached = task.IsNotesAttached;

        //        List<Notes> pnoteslist = new List<Notes>();
        //        var noteslist = db.GnrlNotes.Where(c => c.ObjectId == new Guid(id)).ToList();
        //        foreach (var pnote in noteslist)
        //        {
        //            Notes note = new Notes();
        //            note.NotesId = pnote.NotesId.ToString();
        //            note.FileName = pnote.FileName.ToString();
        //            pnoteslist.Add(note);
        //        }
        //        data.Notes = pnoteslist;

        //        List<MasterProject> projects = new List<MasterProject>();
        //        var p = db.MstrProjects.ToList();
        //        foreach (var project in p)
        //        {
        //            projects.Add(new MasterProject() { ProjectId = project.ProjectId.ToString(), ProjectTitle = project.ProjectTitle });
        //        }
        //        ViewBag.Projects = projects;

        //        List<MasterAssignedTo> users = new List<MasterAssignedTo>();
        //        var u = db.MstrUsers.ToList();
        //        Guid UserRoleId = new Guid("8CE9CBA9-6BD8-4638-9227-800BB500BF7B");
        //        Guid TeamLeadRoleId = new Guid("46F44F42-4ACB-4D45-9DFA-E3BAF70C33F3");

        //        foreach (var user in u)
        //        {
        //            if (user.RoleId != null && (user.RoleId == UserRoleId || user.RoleId == TeamLeadRoleId))
        //            {

        //                users.Add(new MasterAssignedTo() { UserId = user.UserId.ToString(), Name = user.Name });


        //            }
        //        }
        //        ViewBag.Users = users;

        //        List<MasterUserStory> userstories = new List<MasterUserStory>();
        //        var s = db.MstrUserStories.ToList();
        //        foreach (var userStory in s)
        //        {
        //            userstories.Add(new MasterUserStory() { UserStoryId = userStory.UserStoryId.ToString(), Description = userStory.Description });
        //        }
        //        ViewBag.Userstory = userstories;

        //        List<MasterTask> tasks = new List<MasterTask>();
        //        var t = db.MstrTasks.ToList();
        //        foreach (var parenttask in t)
        //        {
        //            tasks.Add(new MasterTask() { TaskId = parenttask.TaskId.ToString(), TaskNo = parenttask.TaskNo });
        //        }
        //        ViewBag.Tasks = tasks;

        //        List<MasterProject> projects1 = new List<MasterProject>();
        //        var p1 = db.MstrProjects.ToList();
        //        foreach (var project in p1)
        //        {
        //            projects1.Add(new MasterProject() { ProjectId = project.ProjectId.ToString(), ProjectTitle = project.ProjectTitle });
        //        }
        //        ViewBag.Projects = projects1;

        //        return View(data);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(String id, FileTask mstrTask)
        //{

        //    var task = db.MstrTasks.FirstOrDefault(x => x.TaskId == new Guid(id));
        //    if (task == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {


        //        task.TaskNo = mstrTask.TaskNo;
        //        task.AssignedTo = mstrTask.AssignedTo;
        //        task.ProjectId = mstrTask.ProjectId;
        //        task.Description = mstrTask.Description;
        //        task.Status = mstrTask.Status.ToString();
        //        task.UserStoryId = mstrTask.UserStoryId;
        //        task.ParentTaskId = mstrTask.ParentTaskId;
        //        task.StartDate = mstrTask.StartDate;
        //        task.EndDate = mstrTask.EndDate;
        //        task.TotalEstimatedHours = mstrTask.TotalEstimatedHours;
        //        task.TotalHoursSpent = mstrTask.TotalHoursSpent;
        //        task.TotalRemainingHours = mstrTask.TotalRemainingHours;
        //        //task.IsNotesAttached = mstrTask.IsNotesAttached;

        //        if (mstrTask.File != null && mstrTask.File.Length > 0)
        //        {
        //            var fileName = mstrTask.File.FileName;

        //            using (var memoryStream = new MemoryStream())
        //            {

        //                byte[] fileData = memoryStream.ToArray();

        //                var gnrlNote = new GnrlNote
        //                {
        //                    NotesId = Guid.NewGuid(),
        //                    ObjectId = task.TaskId,
        //                    FileName = mstrTask.File.FileName,
        //                    Data = fileData,
        //                    LastModifiedOn = DateTime.Now,

        //                };

        //                db.GnrlNotes.Add(gnrlNote);
        //                if (task.IsNotesAttached != true)
        //                {
        //                    task.IsNotesAttached = true;
        //                }
        //            }
        //            ViewBag.FileName = fileName;

        //        }

        //        db.Entry(task).State = EntityState.Modified;
        //        db.SaveChanges();

        //    }
        //    return RedirectToAction("Task");
        //}
        public IActionResult DownloadFile(Guid noteId)
        {

            var note = db.GnrlNotes.FirstOrDefault(n => n.NotesId == noteId);

            if (note == null)
            {
                return NotFound();
            }
            return File(note.Data, "application/octet-stream", note.FileName);
        }
        public IActionResult Edit(string id)
        {
            var task = db.MstrTasks.Include("Project").FirstOrDefault(x => x.TaskId == new Guid(id));
            //var task = db.MstrTasks.FirstOrDefault(x => x.TaskId == new Guid(id));
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

                #region timesheet code
                string loginUserId = HttpContext.Session.GetString("UserID");
                if (!string.IsNullOrEmpty(loginUserId))
                {
                    task.TotalEstimatedHours = task.TotalEstimatedHours;
                    if (task.TotalHoursSpent == task.TotalHoursSpent)
                    {
                    }
                    else
                    {
                        // here we need to insert record in MstrTimesheet
                        MstrTimeSheet timesheet = new MstrTimeSheet();
                        timesheet.TimeSheetId = Guid.NewGuid();
                        timesheet.UserId = new Guid(loginUserId);
                        timesheet.TaskId = new Guid(id);
                        timesheet.ProjectId = task.ProjectId;
                        timesheet.Date = DateTime.Today;
                        timesheet.Status = true;
                        timesheet.Hours = task.TotalHoursSpent - task.TotalHoursSpent;
                        timesheet.ApprovedBy = task.Project.ManagerId;
                        timesheet.AddedOn = DateTime.Today;
                        db.MstrTimeSheets.Add(timesheet);
                        db.SaveChanges();
                    }
                }
                #endregion
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
        public IActionResult Edit(System.String id, FileTask mstrTask)
        {
            var task = db.MstrTasks.Include("Project").FirstOrDefault(x => x.TaskId == new Guid(id));

            //var task = db.MstrTasks.FirstOrDefault(x => x.TaskId == new Guid(id));
            if (task == null)
            {
                return NotFound();
            }
            else
            {


                //task.TaskNo = mstrTask.TaskNo;
                //task.AssignedTo = mstrTask.AssignedTo;
                //task.ProjectId = mstrTask.ProjectId;
                //task.Description = mstrTask.Description;
                //task.Status = mstrTask.Status.ToString();
                //task.UserStoryId = mstrTask.UserStoryId;
                //task.ParentTaskId = mstrTask.ParentTaskId;
                //task.StartDate = mstrTask.StartDate;
                //task.EndDate = mstrTask.EndDate;
                //task.TotalEstimatedHours = mstrTask.TotalEstimatedHours;

                #region timesheet code
                string loginUserId = HttpContext.Session.GetString("UserID");
                if (!string.IsNullOrEmpty(loginUserId))
                {
                    task.TotalEstimatedHours = mstrTask.TotalEstimatedHours;
                    if (task.TotalHoursSpent == mstrTask.TotalHoursSpent)
                    {
                    }
                    else
                    {
                        // here we need to insert record in MstrTimesheet
                        MstrTimeSheet timesheet = new MstrTimeSheet();
                        timesheet.TimeSheetId = Guid.NewGuid();
                        timesheet.UserId = new Guid(loginUserId);
                        timesheet.TaskId = new Guid(id);
                        timesheet.ProjectId = mstrTask.ProjectId;
                        timesheet.Date = DateTime.Today;
                        timesheet.Status = true;
                        timesheet.Hours = mstrTask.TotalHoursSpent - task.TotalHoursSpent;
                        timesheet.ApprovedBy = task.Project.ManagerId;
                        timesheet.AddedOn = DateTime.Today;
                        db.MstrTimeSheets.Add(timesheet);
                        db.SaveChanges();
                    }
                }
                #endregion

                task.TotalHoursSpent = mstrTask.TotalHoursSpent;
                task.TotalRemainingHours = mstrTask.TotalEstimatedHours-mstrTask.TotalHoursSpent;
                //if (task.TotalEstimatedHours == task.TotalHoursSpent)
                //{
                //    task.Status = "Completed";
                //}
                if (task.TotalHoursSpent == 0)
                {
                    task.Status = "NotStarted";
                }
                else if(task.TotalRemainingHours > 0)
                {
                    task.Status = "Pending";
                }
                else
                {
                    task.Status = "Completed";
                }


                //task.IsNotesAttached = mstrTask.IsNotesAttached;

                //if (mstrTask.File != null && mstrTask.File.Length > 0)
                //{
                //    var fileName = mstrTask.File.FileName;

                //    using (var memoryStream = new MemoryStream())
                //    {

                //        byte[] fileData = memoryStream.ToArray();

                //        var gnrlNote = new GnrlNote
                //        {
                //            NotesId = Guid.NewGuid(),
                //            ObjectId = task.TaskId,
                //            FileName = mstrTask.File.FileName,
                //            Data = fileData,
                //            LastModifiedOn = DateTime.Now,

                //        };

                //        db.GnrlNotes.Add(gnrlNote);
                //        if (task.IsNotesAttached != true)
                //        {
                //            task.IsNotesAttached = true;
                //        }
                //    }
                //    ViewBag.FileName = fileName;

                //}

                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();

            }
            return RedirectToAction("Task", new { id = task.UserStoryId });
        }
        public IActionResult Details(Guid id)
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

