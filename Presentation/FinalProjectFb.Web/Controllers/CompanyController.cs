using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectFb.Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }
        public async Task<IActionResult> ConfirmationForm()
        {
            var confirmationFormVM = await _service.GetCitiesForConfirmationFormAsync();
            return View(confirmationFormVM);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmationForm(ConfirmationFormVM vm)
        {
            if (await _service.GetConfirmationFormAsync(vm, ModelState))
                return RedirectToAction(nameof(Index));

            return View(vm);
        }



    }
}
