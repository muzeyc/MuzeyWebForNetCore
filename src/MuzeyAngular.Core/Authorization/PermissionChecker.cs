using Abp.Authorization;
using MuzeyAngular.Authorization.Roles;
using MuzeyAngular.Authorization.Users;

namespace MuzeyAngular.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
