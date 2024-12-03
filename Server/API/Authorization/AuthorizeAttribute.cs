using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;
        public AuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //skip allow anonymous
            var allowAnonymous = context.ActionDescriptor
                .EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            var user = context.HttpContext.User;
            if(user == null || !user.Claims.Any())
            {
                context.Result = new UnauthorizedResult();
            }
            else if(!_roles.Any())
            {
                return;
            }    
            else
            {
                var canAccess = false;
                foreach (var role in _roles)
                {
                    if (user.IsInRole(role))
                    {
                        canAccess = true;
                        break;
                    }
                }

                if(!canAccess)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
