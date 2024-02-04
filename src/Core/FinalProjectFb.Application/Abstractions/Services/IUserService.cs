
using FinalProjectFb.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task Register(RegisterVM registerVM);

        Task Login(LoginVM loginVM);
    }
}
