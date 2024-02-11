﻿using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.Abstractions.Services
{
    public interface ICompanyService
    {
        Task<bool> GetConfirmationFormAsync(ConfirmationFormVM vm,ModelStateDictionary modelstate);
        Task<ConfirmationFormVM> GetCitiesForConfirmationFormAsync(ConfirmationFormVM confirmationFormVM);

        Task<PaginateVM<Company>> GetAllAsync(int page = 1, int take = 10);
    }
}
