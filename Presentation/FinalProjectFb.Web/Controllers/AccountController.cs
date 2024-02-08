﻿using FinalProjectFb.Application.Abstractions.Services;
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
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var result = await _service.Register(vm);
            if (result.Any())
            {
                foreach (var item in result)
                {
                    ModelState.AddModelError(String.Empty, item);
                    return View(vm);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _service.Login(vm);

            if (result.Any())
            {
                foreach (var item in result)
                {
                    ModelState.AddModelError(String.Empty, item);
                }

               
                return View(vm);
            }

            
            if (User.IsInRole("Admin"))
            {               
                return RedirectToAction("Index", "Home");
            }
          
            return RedirectToAction("Index", "Dashboard", new { Area = "manage" });
        }



        public async Task<IActionResult> Logout()
        {
            await _service.Logout();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            await _service.CreateRoleAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateAdminRole()
        {
            await _service.CreateAdminRoleAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
