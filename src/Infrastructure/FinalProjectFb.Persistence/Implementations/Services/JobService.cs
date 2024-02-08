using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Repositories.Generic;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Persistence.Implementations.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Persistence.Implementations.Services
{
    internal class JobService : IJobService
    {
        private readonly IJobRepository _repository;

        public JobService(IJobRepository repository)
        {
            _repository = repository;
        }
        public async Task<JobDetailVM> DetailAsync(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException("id");

            Job job = await _repository.GetByIdAsync(id, includes: new string[] { nameof(Job.Company)
                , nameof(Job.Category)
                , nameof(Job.Images) 
                ,nameof(Job.BasicFunctionsİnfos)
                , nameof(Job.Requirementsİnfos) });

            
            JobItemVM jobItemVM = new JobItemVM
            {
                Vacancy = job.Vacancy,
                JobNature = job.JobNature,
                BasicFunctionsİnfos = job.BasicFunctionsİnfos,
                Requirementsİnfos = job.Requirementsİnfos,
                PostedDate = job.PostedDate,
                Category = job.Category,
                Deadline = job.Deadline,
                DiscontinuationDate = job.DiscontinuationDate,
                Salary = job.Salary,
                Experience = job.Experience,
                Company = job.Company,
                Images = job.Images,

            };
            if (job is null) throw new Exception("Not found");

            List<Job> relatedJobList = await _repository.GetAll(includes:new string[] {nameof(Job.Images)})
               .Where(p => p.CategoryId == job.CategoryId && p.Id != id)               
               .ToListAsync();


            List<Job> companyjoblist = await _repository.GetAll(includes: new string[] { nameof(Job.Images) })
               .Where(p => p.CompanyId == job.CompanyId && p.Id != id)              
               .ToListAsync();

            JobDetailVM detailVM = new JobDetailVM
            {
                RelatedJobs = relatedJobList,
                CompanyJobs = companyjoblist,
                Job = jobItemVM
            };
            return detailVM;
        }
    }
}
