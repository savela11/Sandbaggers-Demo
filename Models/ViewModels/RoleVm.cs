using System.Collections.Generic;

namespace Models.ViewModels
{
    // public class RoleVm
    // {
    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //    public List<UserWithRoleVm> Users { get; set; }
    //    public List<UserWithRoleVm> UsersWithoutRole { get; set; }
    // }

    public class RoleVm
    {
      public string RoleName { get; set; }
      public List<UserWithRoleVm> Members { get; set; }
      public List<UserWithRoleVm> NonMembers { get; set; }
    }

    public class UserWithRoleVm
    {
        public string Id { get; set; }
        public string FullName { get; set; }
    }
}
