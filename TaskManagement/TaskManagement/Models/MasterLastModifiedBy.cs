namespace TaskManagement.Models
{
    public class MasterLastModifiedBy
    {
        public String? UserId { get; set; }

        public string? RoleId { get; set; }
        public string? Name { get; set; }

        private const string ManagerRoleId = "726EF062-88DA-4D2D-9791-50239DCB2142";
        public IEnumerable<string> GetManagerIds(IEnumerable<MasterManager> users)
        {
            return users
                .Where(user => user.RoleId == ManagerRoleId)
                .Select(user => user.UserId);
        }
    }
}
