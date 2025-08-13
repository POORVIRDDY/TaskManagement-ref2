namespace TaskManagement.Models
{
    public class MasterOwnerId
    {
        public String? UserId { get; set; }
        public string? RoleId { get; set; }
        public string? Name { get; set; }

        private const string UserRoleId = "8CE9CBA9-6BD8-4638-9227-800BB500BF7B";
        private const string TeamLeadRoleId = "46F44F42-4ACB-4D45-9DFA-E3BAF70C33F3";
        public IEnumerable<string> GetManagerIds(IEnumerable<MasterManager> users)
        {
            return users
                .Where(user => user.RoleId == UserRoleId || user.RoleId == TeamLeadRoleId)
                .Select(user => user.UserId);
        }
    }
}
