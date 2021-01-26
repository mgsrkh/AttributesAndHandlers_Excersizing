using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;


            var routeList = new List<string>() { "Get", "Post", "Put","Patch" };
            if (context.HasSucceeded)
            {
                context.User.Claims.Any(x => x.Type == routeList.ToString()); 
                context.User.HasClaim(x => x.Type == "Routes");
            }
            return Task.CompletedTask;
        }
    }
}
