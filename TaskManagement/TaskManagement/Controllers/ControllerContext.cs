using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    public class ControllerContext : Controller
    {
        public ActionResult Index()
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string layout;

            // Determine the layout based on the controller name
            switch (controllerName)
            {
                case "Registration":
                    layout = "_Layout1";
                    break;
                case "Login":
                    layout = "_Layout1";
                    break;
                case "AdministratorLanding":
                    layout = "_Layout";
                    break;
                case "ProjectManagerLanding":
                    layout = "_Layout2";
                    break;
                case "Timesheets":
                    layout = "_Layout2";
                    break;
                case "TeamLeadLanding":
                    layout = "_Layout3";
                    break;
                case "UserLanding":
                    layout = "_Layout4";
                    break;
                case "Charts":
                    layout = "_Layout5";
                    break;
                //case "Charts":
                //case "TimeSheets":
                //    layout = "_Layout";
                //    break;
                default:
                    layout = "_Layout1";
                    break;
            }

            // Pass the layout to the view via ViewBag
            ViewBag.Layout = layout;

            return View();
        }

    }
}