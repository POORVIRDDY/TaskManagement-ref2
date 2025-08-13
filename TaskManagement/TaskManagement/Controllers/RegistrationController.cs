using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TaskManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Controllers
{
   
    public class RegistrationController : Controller
    {
        private readonly TaskManagementContext db;

        public RegistrationController(TaskManagementContext context)
        {
            db = context;
        }


        public IActionResult Registration()
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
        public IActionResult Registration(MstrUser mstrUser, LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(mstrUser);
            }
            string encryptedPassword = AesOperation.Encrypt(model.Password);
            MstrUser user = new MstrUser();
            user.UserId = Guid.NewGuid();
            user.Name = mstrUser.Name;
            user.Nationality = mstrUser.Nationality;
            user.DateOfBirth = mstrUser.DateOfBirth;
            user.RoleId = mstrUser.RoleId;
            user.Email = mstrUser.Email;
            user.Password = encryptedPassword;
            user.PhoneNo = mstrUser.PhoneNo;
            user.IsUserActivated = mstrUser.IsUserActivated;

            db.Entry(user).State = EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Login", "Login");
        }
        
    }
}
