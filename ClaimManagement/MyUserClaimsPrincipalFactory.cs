using HelpDeskSystem.Data;
using HelpDeskSystem.Data.Migrations;
using HelpDeskSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security;
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

            // Get Roles
            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Any())
            {
                var userRole = userRoles.First();

                // Find the role from the context
                var role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == userRole);
                if (role != null)
                {
                    // Get permission from the role

                    var Premissions = await _context.UserRoleProfiles
                        .Where(urp => urp.RoleId == role.Id)
                    .Select(urp => $"{urp.Task.Parent.Code}:{urp.Task.Name}")
                    .ToListAsync();

                    var allUserPermissions = "";

                    foreach ( var right in Premissions )
                    {
                        allUserPermissions += $"|{right?.ToUpper()}";
                    }

                    // Add permission Claim
                    identity.AddClaim(new Claim("UserPermission", allUserPermissions));
                }
            }
            return identity;

        }
    }
}
