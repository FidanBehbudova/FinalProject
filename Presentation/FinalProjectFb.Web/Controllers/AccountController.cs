using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectFb.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _service;

        public AccountController(IUserService service)
        {
            _service = service;
        }
        public IActionResult Register()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Register([FromForm] RegisterVM registerVM)
        //{
        //    await _service.Register(registerVM);
        //    return StatusCode(StatusCodes.Status204NoContent);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Login([FromForm] LoginVM loginVM)
        //{

        //    return StatusCode(StatusCodes.Status200OK, await _service.Login(loginVM));
        //}
    }
}
