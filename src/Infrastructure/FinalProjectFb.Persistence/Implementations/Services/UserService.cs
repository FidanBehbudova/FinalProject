using AutoMapper;
using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Services;

using FinalProjectFb.Application.ViewModels.Users;
using FinalProjectFb.Domain.Entities;
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
	

        public UserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
			
        }

        public async Task Login(LoginVM loginVM)
        {
            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if (user is null) throw new Exception("sehv");
            }
            if (!await _userManager.CheckPasswordAsync(user, loginVM.Password)) throw new Exception("sehv");
  
        }

        public async Task Register(RegisterVM registerVM)
        {
            if (await _userManager.Users.AnyAsync(u => u.UserName == registerVM.Username || u.Email == registerVM.Email)) throw new Exception("bunnan bizde var");
            AppUser appUser = _mapper.Map<AppUser>(registerVM);
            var result = await _userManager.CreateAsync(appUser, registerVM.Password);
            if (!result.Succeeded)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in result.Errors)
                {
                    stringBuilder.AppendLine(item.Description);
                }
                throw new Exception(stringBuilder.ToString());
            }

        }
    }
}
