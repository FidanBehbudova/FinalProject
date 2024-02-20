using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Persistence.Implementations.Services;
using FinalProjectFb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectFb.Web.Controllers
{
	public class CvController : Controller
	{
		private readonly ICvService _service;

		public CvController(ICvService service)
        {
			_service = service;
		}
        
        public async Task<IActionResult> Index(int id, int page = 1, int take = 10)
        {
            PaginateVM<Cv> vm = await _service.GetAllAsync(id, page, take);
            vm.JobId = id;
            if (vm.Items == null) return NotFound();
            return View(vm);
        }
        
		[Authorize(Roles = "Member")]
		public IActionResult Create(int jobId)
		{
			CreateCvVM vm = new CreateCvVM
			{
				JobId = jobId,
			};
			
			return View(vm);
		}
        [HttpPost]
		[Authorize(Roles = "Member")]

		public async Task<IActionResult> Create(CreateCvVM vm)
        {
           
            Console.WriteLine($"JobId: {vm.JobId}");

            if (vm.JobId > 0)
            {
               
                if (await _service.CreateAsync(vm, ModelState))
                    return RedirectToAction("Index","Home", new { id = vm.JobId });
            }

            return View(vm);
        }
		public async Task<IActionResult> Detail(int id)
		{
			return View(await _service.DetailAsync(id));
		}
        public async Task<IActionResult> Deleted(int id)
        {
            await _service.SoftDeleteAsync(id);
			Cv cv = await _service.GetJobAsync(id);

			return RedirectToAction("Index", new { Id = cv.Job.Id });
		}
        public async Task<IActionResult> Confirmed(int id)
        {
            await _service.ReverseDeleteAsync(id);
           Cv cv =await _service.GetJobAsync(id);

            return RedirectToAction("Index",new {Id=cv.Job.Id});
        }
    }
}
