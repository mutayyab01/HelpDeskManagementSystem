using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace HelpDeskSystem.ClaimManagement
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MyUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
            _userManager = userManager;
            _context = context;

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            var userRoles = await _userManager.GetRolesAsync(user);

            var userrole = userRoles.FirstOrDefault();
            var allUserPremission = "";

            if (userrole != null)
            {
                var userRoleId = await _context.Roles.SingleAsync(i => i.Name == userrole);
                var userPremission = await _context.UserRoleProfiles.Where(x => x.RoleId == userRoleId.Id)
                    .Select(p => p.Task.Parent.Name + ":" + p.Task.Name).ToListAsync();

                foreach (var premission in userPremission)
                {
                    allUserPremission += $"@{premission}";
                }

            }
            identity.AddClaim(new Claim("UserPermission", allUserPremission));

            return identity;

        }
    }
}
