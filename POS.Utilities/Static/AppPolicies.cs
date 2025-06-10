using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Utilities.Static
{
    public static class AppPolicies
    {
        public const string ApiKeyPolicy = "ApiKeyPolicy"; 
        public const string RequireAdminRole = "RequireAdminRole";
        public const string CanManageCustomers = "CanManageCustomers";
        public const string ViewReports = "ViewReports";
    }
}