using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Models
{
    public class SessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userId = filterContext.HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userId)) 
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                { "Controller", "Login" },
                { "Action", "Login" }
                    });
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }   
            
        }
    }
}
