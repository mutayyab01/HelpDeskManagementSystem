using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace HelpDeskSystem.Services
{
    public static class UserAccessService
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return null;

            }
            else
            {
                ClaimsPrincipal CurrentlogggedinUser = user;
                if (CurrentlogggedinUser != null)
                {
                    return CurrentlogggedinUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                }
                else
                {
                    return null;

                }
            }

        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return null;

            }
            else
            {
                ClaimsPrincipal CurrentlogggedinUser = user;
                if (CurrentlogggedinUser != null)
                {
                    return CurrentlogggedinUser.FindFirstValue(ClaimTypes.Name);
                }
                else
                {
                    return null;

                }
            }

        }

        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return null;

            }
            else
            {
                ClaimsPrincipal CurrentlogggedinUser = user;
                if (CurrentlogggedinUser != null)
                {
                    return CurrentlogggedinUser.FindFirstValue(ClaimTypes.Email);
                }
                else
                {
                    return null;

                }
            }

        }

        public static string GetUserRoleId(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return null;

            }
            else
            {
                ClaimsPrincipal CurrentlogggedinUser = user;
                if (CurrentlogggedinUser != null)
                {
                    return CurrentlogggedinUser.FindFirst("RoleId").Value;
                }
                else
                {
                    return null;

                }
            }

        }
    }
}
