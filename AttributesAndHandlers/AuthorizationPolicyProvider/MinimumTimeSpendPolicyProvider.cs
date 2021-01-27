using AttributesAndHandlers.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AttributesAndHandlers.AuthorizationPolicyProvider
{
    public class MinimumTimeSpendPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider DefaultPolicyProvider { get; }
        public MinimumTimeSpendPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            DefaultPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return DefaultPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            string[] subStringPolicy = policyName.Split(new char[] { '.' });
            if (subStringPolicy.Length > 1 && subStringPolicy[0].Equals("MinimumTimeSpend", StringComparison.OrdinalIgnoreCase) && int.TryParse(subStringPolicy[1], out var days))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new MinimumTimeSpendRequirement(days));
                return Task.FromResult(policy.Build());
            }
            return DefaultPolicyProvider.GetPolicyAsync(policyName);
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return DefaultPolicyProvider.GetFallbackPolicyAsync();
        }
    }
}

