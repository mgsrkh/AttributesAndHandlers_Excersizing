using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesAndHandlers.Authorization_attributes
{
    internal class NameAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "Claim.Mahyar";

        bool result = false;
        public NameAuthorizeAttribute(string name) => Name = name;

        public string Name
        {
            get
            {
                if (result == true)
                {
                    return Name;
                }
                return default(string);
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }
    }
}
