using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using System.Security.Claims;

namespace HelpDeskSystem.ClaimManagement
{
    public abstract class AttributeAuthorizationHandler<TRequirement, TAttribute> : AuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement where TAttribute : Attribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected AttributeAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            List<PermissionAttribute> attributes = new();
            var actions = _httpContextAccessor.HttpContext.GetEndpoint().Metadata;

            var allPermission = (PermissionAttribute)actions.FirstOrDefault(x => x.GetType() == typeof(PermissionAttribute));

            attributes.Add(allPermission);


            return HandleRequirementAsync(context, requirement, attributes);

        }

        protected abstract Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TRequirement requirement,
            IEnumerable<PermissionAttribute> attributes);

    }


    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        // Add any Custom requirement properties or method if needed
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : AuthorizeAttribute
    {
        public string Name { get; }
        public PermissionAttribute(string name) : base("Permission")
        {
            Name = name;
        }

    }
    public class PermissionAuthorizationHandler : AttributeAuthorizationHandler<PermissionAuthorizationRequirement, PermissionAttribute>
    {
        public PermissionAuthorizationHandler(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionAuthorizationRequirement requirement, IEnumerable<PermissionAttribute> attributes)
        {
            if (attributes == null || !attributes.Any())
            {
                context.Fail();
                return;
            }


            foreach (var permissionattribute in attributes)
            {
                var hasPermission = await AuthorizeAsync(context.User, permissionattribute.Name);
                if (!hasPermission)
                {
                    context.Fail();
                    return;
                }

            }

            context.Succeed(requirement);
        }
        private Task<bool> AuthorizeAsync(ClaimsPrincipal user, string permission)
        {
            var userPermissions = user.FindFirstValue("UserPermission")?.ToLower();
            // Check for permission in user's claims

            var hasPermission = Task.FromResult(userPermissions != null
                && userPermissions.Split('|').Contains(permission.ToLower()));
            //var a = Task.FromResult(userPermissions != null && userPermissions.Split('|'));

            return hasPermission;
        }
    }

}
