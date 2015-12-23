using System.Web;
using System.Web.Mvc;

namespace Service
{
    /// <summary>
    /// This class contain all global filters
    /// </summary>
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }
  }
}