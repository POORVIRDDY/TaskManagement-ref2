# Task Management Web Application

A role-based task management system designed to streamline project tracking and collaboration among administrators, managers, team leads, and users.

---

## Technologies Used

- **ASP.NET MVC** with C#
- **Entity Framework / ADO.NET** (Database First Approach)
- **Encryption & Decryption** for secure data handling
- **Session Management** for user state tracking
- **jQuery** (Basic Queries & UI interaction)
- **jqGrid** for dynamic tabular data display
- **LINQ** basics for querying data collections

---

## Application Flow

### Authentication & Role-Based Access
- **Login** verifies username and password.
- Upon authentication, user role is fetched from the database.
- Based on role, a customized layout/dashboard is shown:

### Administrator
- View all user information.
- Access dashboard displaying active/inactive users and system overview.

### Customer
- Access dashboard showing progress of projects they are associated with.

### Manager
- View list of projects they manage.
- Create new projects.
- Access project details including user stories.
- User stories are assigned and edited by managers for team leads.
- User stories contain tasks and subtasks, assigned to team members.
- Access timesheets and track project progress via dashboard.

### Team Lead
- View user stories assigned to them.
- Assign and edit tasks assigned to team members.

### User
- View assigned tasks, deadlines, and attached documents.
- Log hours spent on tasks.
- Download documents attached by manager or team lead.

---

## Additional Features

- **Document Management:** Managers and team leads can upload project-related documents; users can download.
- **Notification System:** 
  - All roles have a notification alert button.
  - Alerts for impending deadlines on projects, user stories, and tasks.
- **Timesheet Management:** Managers monitor work hours logged by team members.

---

## How to Use

1. **Login** with appropriate credentials.
2. Navigate to respective dashboards based on role.
3. Administrators can manage user accounts.
4. Managers oversee projects, assign user stories, and monitor overall progress.
5. Team leads manage day-to-day task assignment and updates.
6. Users update status and time spent on individual tasks.

---

## Project Structure

- MVC architecture separating concerns for scalability and maintainability.
- Entity Framework Database First ensures seamless integration with existing databases.
- JQuery & jqGrid provide dynamic UI components enhancing user interaction.

---

## Contribution

Contributions, feature requests, and bug reports are welcome. Please fork the repository and submit pull requests for review.

---

## License

This project is for educational and demonstration purposes. Check with your organization for use in production environments.

---

**Task Management Web App** — Efficiently organize, track, and collaborate on projects with role-based control.
