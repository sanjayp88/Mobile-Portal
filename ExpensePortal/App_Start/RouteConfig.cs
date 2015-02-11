using System.Web.Mvc;
using System.Web.Routing;

namespace ExpensePortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Login", "login", new { controller = "Home", action = "Index" }
            );
            routes.MapRoute("Expense", "Expense/{action}/{payrollNumber}/{year}/{week}/{product}", new { controller = "Expense", action = "Index" }

            );
            //routes.MapRoute("Document", "{Document}/{action}/{id}", new { controller = "Document", action = "Index" }
            //  );
            routes.MapRoute("Product", "Expense/Product/{Prodcut}", new { controller = "Expense", action = "Product" });
            routes.MapRoute("Document", "Document/Documents/{CategoryType}", new { controller = "Document", action = "Documents" });

           // routes.MapRoute("DocumentView", "Document/DocumentView/{Name}", new { controller = "Document", action = "DocumentView" });
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
           
        }
    }
}
