using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Utilities.Static
{
    public static class AppPermissions
    {
        // Clientes
        public const string Customers_List = "Customers.List";
        public const string Customers_Create = "Customers.Create";
        public const string Customers_Edit = "Customers.Edit";
        public const string Customers_Delete = "Customers.Delete";
        public const string Customers_Export = "Customers.Export";

        // Productos
        public const string Products_List = "Products.List";
        public const string Products_Create = "Products.Create";
        public const string Products_Edit = "Products.Edit";
        // ...

        // Reportes
        public const string Reports_ViewSales = "Reports.ViewSales";
        public const string Reports_ViewFinancial = "Reports.ViewFinancial";
    }
}