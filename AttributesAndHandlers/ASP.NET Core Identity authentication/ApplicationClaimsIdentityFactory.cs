using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AttributesAndHandlers.ASP.NET_Core_Identity_authentication
{
    public class ApplicationClaimsIdentityFactory : Microsoft.AspNetCore.Identity.UserClaimsPrincipalFactory<IdentityUser>
    {
        UserManager<IdentityUser> _userManager;
        public ApplicationClaimsIdentityFactory(UserManager<IdentityUser> userManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        { }
        public async override Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            var principal = await base.CreateAsync(user);
            if (user.UserName != null)
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim("IsDeveloper", "true")
            });
            }
            return principal;
        }
    }
}
