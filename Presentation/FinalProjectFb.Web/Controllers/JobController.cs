using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.ViewModels;
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
       
        
        public async Task<IActionResult> Create()
        {
            CreateJobVM createJobVM=new CreateJobVM();
            createJobVM = await _service.CreatedAsync(createJobVM);
            return View(createJobVM);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmationForm(CreateJobVM createJobVM)
        {
            //if (await _service.GetConfirmationFormAsync(confirmationFormVM, ModelState))
            //    return RedirectToAction("Index", "Home");
            //return View(await _service.GetCitiesForConfirmationFormAsync(confirmationFormVM));

            if (await _service.Create(createJobVM, ModelState))
                return RedirectToAction(nameof(Index));
            return View(await _service.CreatedAsync(createJobVM));
        }
        //public async Task<IActionResult> Create()
        //{
        //    CreateJobVM createJobVM = new CreateJobVM();
        //    return View(createJobVM);
        //}
    }
}
