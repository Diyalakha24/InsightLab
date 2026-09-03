using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace InsightLab.Web.Controllers
{
    /// <summary>
    /// Minimal controller that only exists to render the friendly error page
    /// configured in Program.cs (app.UseExceptionHandler("/Home/Error")).
    /// </summary>
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(Activity.Current?.Id ?? HttpContext.TraceIdentifier);
        }
    }
}
