using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using TaskManagement.Models;


namespace TaskManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly TaskManagementContext db;
      
        #region ctor
        //public UserController(TaskManagementContext context, Role role)
        public LoginController(TaskManagementContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(context));
            // role=role ?? throw new ArgumentNullException(nameof(role));
        }
        #endregion

        public ActionResult Login(LoginViewModel mstrUser)
        {
            List<MasterRole> roles = new List<MasterRole>();
            var query = db.MstrRoles.ToList();
            foreach (var role in query)
            {
                roles.Add(new MasterRole() { RoleID = role.RoleId.ToString(), Name = role.Name });
            }
            ViewBag.Roles = roles;

            //Set session
            HttpContext.Session.SetString("UserID", string.Empty);
            HttpContext.Session.SetString("RoleName", string.Empty);

            return View();
        }


        

        [HttpPost]
        public ActionResult Login(LoginViewModel model, LoginViewModel mstrUser)
        {
            LoginViewModel user = new LoginViewModel();
            user.RoleId = mstrUser.RoleId;

           
            var dbUser = db.MstrUsers.Include("Role").FirstOrDefault(u => u.Email == model.Email);

            if (dbUser != null)
            {
                //if (!dbUser.IsUserActivated)  
                //{
                //    TempData["ErrorMessage"] = "Your account is not activated. Please contact support.";
                //    return RedirectToAction("Error", "Home");
                //}
                // Decrypt 
                string decryptedPassword = AesOperation.Decrypt(dbUser.Password);

                // Check password 
                if (decryptedPassword == model.Password)
                {
                    // Set session strings
                    HttpContext.Session.SetString("UserID", dbUser.UserId.ToString());
                    HttpContext.Session.SetString("RoleName", dbUser.Role.Name);
                    // Get session strings
                    var RoleName = HttpContext.Session.GetString("RoleName");
                    // Check role for individual Landing pages
                    if (RoleName == "Administrator")
                    {
                        return RedirectToAction("User", "AdministratorLanding");
                    }

                    else if (RoleName == "Manager")
                    {
                        return RedirectToAction("Project", "ProjectManagerLanding");
                    }

                    else if (RoleName == "Team Lead")
                    {
                        return RedirectToAction("UserStory", "TeamLeadLanding");
                    }

                    else if (RoleName == "User ")
                    {
                        return RedirectToAction("Task", "UserLanding");
                    }

                    else if (RoleName == "Customer")
                    {
                        return RedirectToAction("Customer", "Charts");
                    }
                 
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                    return RedirectToAction("Error","Home");  
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid email or password.";
                return RedirectToAction("Error", "Home");  
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Clear session 
            HttpContext.Session.Remove("UserID");
            HttpContext.Session.Remove("RoleName");

            return RedirectToAction("Index", "Home");
        }

        
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Fetch the user based on email
                var dbUser = db.MstrUsers.FirstOrDefault(u => u.Email == model.Email);

                if (dbUser != null)
                {
                    // Encrypt new password
                    string encryptedPassword = AesOperation.Encrypt(model.NewPassword);

                    // Update user password
                    dbUser.Password = encryptedPassword;
                    db.SaveChanges();

                   
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Email address not found.");
                }
            }
            return View(model);
        }



    }
}
