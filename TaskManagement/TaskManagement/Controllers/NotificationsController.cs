using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    [SessionCheck]
    public class NotificationsController : Controller
    {
        private readonly NotificationService _notificationService;

        public NotificationsController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            var UserID = HttpContext.Session.GetString("UserID");
            var userId = UserID?.ToString();

            DateTime today = DateTime.Today;
            DateTime finalDate = today.AddDays(6);

            // Send notifications based on current date
            _notificationService.SendProjectNotifications(today, finalDate);
            _notificationService.SendTaskNotifications(today, finalDate);
            _notificationService.SendUserStoriesNotifications(today, finalDate);

            // Retrieve all notifications for the current user
            var notifications = _notificationService.GetUserNotifications(userId);

            //Filter notifications based on type(Project, Task, UserStory)
            var filteredNotifications = notifications.Select(n => new
            {
                Id = n.NotificationId,
                Subject = n.Subject,
                NotificationDate = DateTime.UtcNow,
                NotificationType = n.Subject.Contains("Project") ? "Project" :
                                    n.Subject.Contains("Task") ? "Task" : "UserStory"
            }).ToList();
            //var filteredNotifications = notifications.Select(n => new
            //{
            //    Id = n.NotificationId,
            //    Subject = n.Subject,
            //    NotificationDate = DateTime.UtcNow,
            //    NotificationType =
            //    n.Subject.IndexOf("Project", StringComparison.OrdinalIgnoreCase) >= 0 ? "Project" :
            //    n.Subject.IndexOf("Task", StringComparison.OrdinalIgnoreCase) >= 0 ? "Task" :
            //    n.Subject.IndexOf("UserStory", StringComparison.OrdinalIgnoreCase) >= 0 ? "UserStory" :
            //    "Other" // Fallback if none of the types match
            //}).ToList();



            return View(filteredNotifications);
        }
    }
}