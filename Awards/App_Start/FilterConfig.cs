using System.Web.Mvc;

namespace Awards.App_Start
{
    public static class FilterConfig
    {
        internal static void RegisterFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
