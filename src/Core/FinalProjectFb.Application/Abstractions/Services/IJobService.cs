using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.Abstractions.Services
{
    public interface IJobService
    {
        Task<JobDetailVM> DetailAsync(int id);
        Task<bool> Create(CreateJobVM createJobVM, ModelStateDictionary modelstate);
        Task<CreateJobVM> CreatedAsync(CreateJobVM vm);
        //Task<JobItemVM> SortingAsync(int key = 1, int page = 1, int id=1);
    }
}
