using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectFb.Web.Controllers
{
    public class JobController : Controller
    {
        private readonly IJobService _service;

        public JobController(IJobService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Detail(int id)
        {           
            return View(await _service.DetailAsync(id));
        }
    }
}
