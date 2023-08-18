using System.Web;
using System.Web.Mvc;
using WebApiApp.Extensions;

namespace WebApiApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
