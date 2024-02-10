using AutoMapper;
using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.Utilities;
using FinalProjectFb.Application.ViewModels.Users;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Persistence.Implementations.Services
{
    internal class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<AppUser> userManager, IMapper mapper,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<List<string>> Register(RegisterVM vm)
        {
            List<string> str = new List<string>();
            if (!vm.Name.IsLetter())
            {
                str.Add("Your Name or Surname only contain letters");
                return str;
            }
            if (!vm.Email.CheeckEmail())
            {
                str.Add("Your Email type is not true");
                return str;
            }
            vm.Name.Capitalize();
            vm.Surname.Capitalize();
            if (!vm.Gender.CheeckGender())
            {
                str.Add("Your Gender is not exis");
                return str;
            }

            AppUser user = new AppUser
            {
                Name = vm.Name,
                UserName = vm.Username,
                Surname = vm.Surname,
                Email = vm.Email,
                Birthday = vm.Birthday,
                Gender = vm.Gender
            };
            IdentityResult result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {

                    str.Add(error.Description);
                }
                return str;
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            if (user != null)
            {
                await AssignRoleToUser(user, vm.Role.ToString());
            }
            return str;

        }
        public async Task AssignRoleToUser(AppUser user, string roleName)
        {
           
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }           
            await _userManager.AddToRoleAsync(user, roleName);
        }
        public async Task<List<string>> Login(LoginVM vm)
        {
            List<string> str = new List<string>();
            AppUser user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
                if (user == null)
                {
                    str.Add("Username, Email or Password was wrong");
                    return str;

                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemembered, true);
            if (result.IsLockedOut)
            {
                str.Add("You have a lot of fail  try that is why you banned please try some minuts late");
                return str;
            }
            if (!result.Succeeded)
            {
                str.Add("Username, Email or Password was wrong");
                return str;
            }
            
            return str;
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task CreateRoleAsync()
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString(),
                    });

                }
            }
        }
        public async Task CreateAdminRoleAsync()
        {
            var adminRoleName = "Admin";

            if (!await _roleManager.RoleExistsAsync(adminRoleName))
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = adminRoleName,
                });
            }
        }

       
    }
}
