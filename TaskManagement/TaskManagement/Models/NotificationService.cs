using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagement.Models
{
    [SessionCheck]
    public class NotificationService
    {
        private readonly TaskManagementContext _context;

        public NotificationService(TaskManagementContext context)
        {
            _context = context;
        }

        public void SendProjectNotifications(DateTime today, DateTime finalDate)
        {
            // Fetch projects that are ending soon and have an active status
            var projectsToNotify = _context.MstrProjects
                .Where(p => p.EndDate >= today && p.EndDate <= finalDate && p.Status == "Active" && p.ManagerId != null)
                .ToList();

            // Loop through the projects and send notifications
            foreach (var project in projectsToNotify)
            {
                AddNotification(_context, project.ManagerId.Value,
                    $"Project '{project.ProjectTitle}' is ending soon!");
            }
        }

        public void SendTaskNotifications(DateTime today, DateTime finalDate)
        {
            // Notify for tasks
            var tasksToNotify = _context.MstrTasks
                .Where(t => t.EndDate >= today && t.EndDate <= finalDate && t.Status != "Completed" && t.AssignedTo != null)
                .ToList();

            foreach (var task in tasksToNotify)
            {
                AddNotification(_context, task.AssignedTo.Value,
                    $"Task '{task.TaskNo}' is ending soon!");
            }
        }
        public void SendUserStoriesNotifications(DateTime today, DateTime finalDate)
        {

            // Notify for user stories
            var userStoriesToNotify = _context.MstrUserStories
                .Where(us => us.Project != null && us.Project.EndDate >= today && us.Project.EndDate <= finalDate)
                .ToList();

            foreach (var userStory in userStoriesToNotify)
            {
                AddNotification(_context, userStory.OwnerId.Value,
                    $"Userstory in project '{userStory.Project?.ProjectTitle}' is ending soon!");
            }
        }

        private static void AddNotification(TaskManagementContext context, Guid userId, string subject)
        {
            var notification = new MstrNotification
            {
                NotificationId = Guid.NewGuid(),
                Subject = subject,
                NotificationDate = DateTime.UtcNow,
                UserId = userId
            };

            context.MstrNotifications.Add(notification);
            context.SaveChanges();
        }

        public List<MstrNotification> GetUserNotifications(string userId)
        {
            // Assuming userId is stored as a string (convert if necessary)
            return _context.MstrNotifications
                .Where(n => n.UserId.ToString() == userId)
                .OrderByDescending(n => n.NotificationDate)
                .ToList();
        }
    }
}