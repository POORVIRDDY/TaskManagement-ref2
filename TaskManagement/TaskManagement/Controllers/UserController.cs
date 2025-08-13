using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    [SessionCheck]
    public class UserController : Controller
    {
        private readonly TaskManagementContext db;
        // private readonly Role role;

        #region ctor
        //public UserController(TaskManagementContext context, Role role)
        public UserController(TaskManagementContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(context));
            // role=role ?? throw new ArgumentNullException(nameof(role));
        }
        #endregion




        /// <summary>
        /// This gives list of all the users.
        /// </summary>
        /// <returns></returns>
       public IActionResult User()
        {
            var users = db.MstrUsers
                .Include(u => u.Role)
                .Select(u => new
                {
                    u.UserId,
                    u.Name,
                    u.DateOfBirth,
                    u.Nationality,
                    u.PhoneNo,
                    u.Email,
                    RoleName = u.Role.Name,
                    u.IsUserActivated
                }).ToList();

            ViewBag.Users = users;  // Passing data to the view
            return View();
        }
        public IActionResult Create()
        {
            List<MasterRole> roles = new List<MasterRole>();
            var query = db.MstrRoles.ToList();
            foreach (var role in query)
            {
                roles.Add(new MasterRole() { RoleID = role.RoleId.ToString(), Name = role.Name });
            }
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        public IActionResult Create(MstrUser mstrUser)
        {
            if (!ModelState.IsValid)
            {
                return View(mstrUser);
            }

            MstrUser user = new MstrUser();
            user.UserId = Guid.NewGuid();
            user.Name = mstrUser.Name;
            user.Nationality = mstrUser.Nationality;
            user.DateOfBirth = mstrUser.DateOfBirth;
            user.RoleId = mstrUser.RoleId;
            user.Email = mstrUser.Email;
            user.Password = mstrUser.Password;
            user.PhoneNo = mstrUser.PhoneNo;
            user.IsUserActivated = mstrUser.IsUserActivated;

            db.Entry(user).State = EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("User");
        }



        /// <summary>
        /// User view in EDIT mode
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(Guid id)
        {
            var user = db.MstrUsers.FirstOrDefault(x => x.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            List<MasterRole> roles = new List<MasterRole>();
            var query = db.MstrRoles.ToList();
            foreach (var role in query)
            {
                roles.Add(new MasterRole() { RoleID = role.RoleId.ToString(), Name = role.Name });
            }
            ViewBag.Roles = roles;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, MstrUser mstrUser)
        {
            // if the mode is invalid
            if (!ModelState.IsValid)
            {
                return View(mstrUser);
            }

            var user = db.MstrUsers.FirstOrDefault(x => x.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.Name = mstrUser.Name;
                user.Nationality = mstrUser.Nationality;
                user.DateOfBirth = mstrUser.DateOfBirth;
                user.RoleId = mstrUser.RoleId;
                user.Email = mstrUser.Email;
                //user.Password = mstrUser.Password;
                user.PhoneNo = mstrUser.PhoneNo;
                user.IsUserActivated = mstrUser.IsUserActivated;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("User");
            }
        }




        public IActionResult Details(Guid id)
        {
           
            var user = db.MstrUsers.Include(u => u.Role).FirstOrDefault(x => x.UserId == id);

            if (user == null)
            {
                return NotFound(); 
            }

            return View(user);
        }










        //public IActionResult Delete(Guid id)
        //{
        //    var user = db.MstrUsers.FirstOrDefault(x => x.UserId == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    db.MstrUsers.Remove(user);
        //    db.SaveChanges();
        //    return RedirectToAction("User"); 
        //}




        public IActionResult Delete(Guid id)
        {
           
            var user = db.MstrUsers.FirstOrDefault(x => x.UserId == id);
            if (user != null)
            {
                user.IsUserActivated = false;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("User");
        }


        // create one view for user not found.
        public IActionResult UserNotFound()
        {
            return View();
        }
    }
}
