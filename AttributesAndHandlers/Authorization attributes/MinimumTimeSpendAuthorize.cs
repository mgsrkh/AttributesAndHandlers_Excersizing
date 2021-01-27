using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesAndHandlers.Authorization_attributes
{
    internal class MinimumTimeSpendAuthorize : AuthorizeAttribute
    {
        public MinimumTimeSpendAuthorize(int days)
        {
            NoOfDays = days;
        }

        int days;

        public int NoOfDays
        {
            get
            {
                return days;
            }
            set
            {
                days = value;
                Policy = $"{"MinimumTimeSpend"}.{value.ToString()}";
            }
        }
    }
}
