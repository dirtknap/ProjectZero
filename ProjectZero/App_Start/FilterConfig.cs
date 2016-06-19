using System.Web.Mvc;
using ProjectZero.Utils;

namespace ProjectZero
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequireSecureConnectionFilter());
        }
    }
}
