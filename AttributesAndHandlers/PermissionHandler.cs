using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AttributesAndHandlers
{
    public class PermissionHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirements = context.PendingRequirements.ToList();

            foreach (var requirement in pendingRequirements)
            {
                if (requirement != null)
                {
                    if (IsOwner(context.User, context.Resource) ||
                        IsSponsor(context.User, context.Resource))
                    {
                        context.Succeed(requirement);
                    }
                }
                else
                {
                    if (IsOwner(context.User, context.Resource))
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            return Task.CompletedTask;
        }

        private bool IsOwner(ClaimsPrincipal user, object resource)
        {
            var userClaim = user.FindFirst(ClaimTypes.Name);
            if (userClaim != null)
            {
                return true;
            }
            return false;
        }

        private bool IsSponsor(ClaimsPrincipal user, object resource)
        {
            var userClaim = user.FindFirst(ClaimTypes.Name);

            if (userClaim != null)
            {
                return true;
            }
            return false;
        }
    }
}
