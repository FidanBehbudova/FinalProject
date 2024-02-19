using FinalProjectFb.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectFb.Web.Controllers
{
	public class BaseController : Controller
	{
		public ActionResult RedirectToIndexBasedOnRole()
		{
			if (User.IsInRole(("Admin")))
			{
				return RedirectToAction("Index", "Dashboard", new { area = "manage" });
			}
			else if (User.IsInRole(UserRole.Creater.ToString()))
			{
				return RedirectToAction("Index", "Home");
			}
			else if (User.IsInRole(UserRole.Member.ToString()))
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}
	}
}
