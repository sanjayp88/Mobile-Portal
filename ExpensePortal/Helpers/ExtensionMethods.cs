using System.Web.Mvc;

namespace ExpensePortal.Helpers
{
    public static class ExtensionMethods
    {
        public static MvcHtmlString If(this MvcHtmlString value, bool evaluation)
        {
            return evaluation ? value : MvcHtmlString.Empty;
        }
    }
}