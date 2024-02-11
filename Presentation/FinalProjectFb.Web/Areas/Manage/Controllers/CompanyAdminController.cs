using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectFb.Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CompanyAdminController : Controller
    {
        private readonly ICompanyService _service;

        public CompanyAdminController(ICompanyService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 10)
        {
            PaginateVM<Company> vm = await _service.GetAllAsync(page, take);
            if (vm.Items == null) return NotFound();
            return View(vm);
        }
    }
}
