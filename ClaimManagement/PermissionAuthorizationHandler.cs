using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using System.Security.Claims;

namespace HelpDeskSystem.ClaimManagement
{
    public abstract class AttributeAuthorizationHandler<TRequirement, TAttribute> : AuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement where TAttribute : Attribute
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            var attributes = new List<PermissionAttribute>();
            var action = (context.Resource as Endpoint)?.Metadata.ToList();
            if (action != null)
            {
                var perm = (PermissionAttribute)action.FirstOrDefault(i => i.GetType().FullName == "PermissionAttribute");
            }
            return HandleRequirementAsync(context, requirement, attributes);
        }
        //private readonly IHttpContextAccessor _contextAccessor;

        //public AttributeAuthorizationHandler(IHttpContextAccessor contextAccessor)
        //{
        //    _contextAccessor = contextAccessor;
        //}
        protected abstract Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement, IEnumerable<PermissionAttribute> attributes);

        private static IEnumerable<TAttribute> GetAttributes(MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>();

        }
    }


    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        // Add any Custom requirement
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
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement, IEnumerable<PermissionAttribute> attributes)
        {
            foreach (var permissionattribute in attributes)
            {
                if (!await AuthorizeAsync(context.User, permissionattribute.Name))
                {
                    return;
                }
            }

            context.Succeed(requirement);
        }
        private Task<bool> AuthorizeAsync(ClaimsPrincipal user, string permission)
        {
            var userProfile = user.FindFirstValue("UserPermission")?.ToLower();
            if (string.IsNullOrEmpty(userProfile))
                return Task.FromResult(false);
            try
            {
                if (userProfile.Contains(permission.ToLower()))
                {
                    return Task.FromResult(true);
                }
            }
            catch (Exception e)
            {
                return Task.FromResult(false);

            }
            return Task.FromResult(false);
        }
    }

}
