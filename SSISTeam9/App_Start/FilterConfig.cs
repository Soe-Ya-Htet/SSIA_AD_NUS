using SSISTeam9.Filters;
using System.Web.Mvc;

namespace SSISTeam9
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
