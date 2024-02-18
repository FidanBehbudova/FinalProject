using Microsoft.AspNetCore.Mvc;

namespace FinalProjectFb.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ErrorPage(string error)
        {
            return View(model: error);
        }
    }
}
