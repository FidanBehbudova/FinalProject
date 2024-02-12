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
            ConfirmationFormVM confirmationFormVM = new ConfirmationFormVM();
            confirmationFormVM.CityIds = new List<int>(); 
            confirmationFormVM = await _service.GetCitiesForConfirmationFormAsync(confirmationFormVM);
            return View(confirmationFormVM);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmationForm(ConfirmationFormVM confirmationFormVM)
        {
            if (await _service.GetConfirmationFormAsync(confirmationFormVM, ModelState))
                return RedirectToAction("Index","Home");
            return View(await _service.GetCitiesForConfirmationFormAsync(confirmationFormVM));
        }




    }
}
