using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Repositories.Generic;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Persistence.DAL;
using FinalProjectFb.Persistence.Implementations.Repositories.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

            Job job = await _repository.GetByIdAsync(id, includes: new string[] {"Company", "Company.CompanyCities", "Company.CompanyCities.City", "Category","Images", "BasicFunctionsİnfos" , "Requirementsİnfos" });

           
            JobItemVM jobItemVM = new JobItemVM
            {
                Vacancy = job.Vacancy,
                JobNature = job.JobNature,
                BasicFunctionsİnfos = job.BasicFunctionsİnfos,
                Requirementsİnfos = job.Requirementsİnfos,
                Name = job.Name, 
                CreatedAt = job.CreatedAt,
                Category = job.Category,
                Deadline = job.Deadline,
                DiscontinuationDate = job.DiscontinuationDate,
                Salary = job.Salary,
                Experience = job.Experience,
                Company = job.Company,
                Images = job.Images,
                CompanyId = job.CompanyId,
                CategoryId = job.CategoryId,
                

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

        //public async Task<JobItemVM> SortingAsync(int key = 1, int page = 1, int id = 1)
        //{
        //    if (id < 1) throw new ArgumentOutOfRangeException("id");

        //    List<Job> jobs=await _repository.GetAll().Include(j => j.Images)
        //        .Include(j => j.Category)
        //        .Include(j => j.Company)
        //        .Include(j => j.Company).ThenInclude(c => c.CompanyCities).ThenInclude(c => c.City)
        //        .ToListAsync();


        //    int count = await _repository.GetAll().CountAsync();

        //    int itemsPerPage = 3;


        //    double totalPages = Math.Ceiling((double)count / itemsPerPage);
        //    if (page <= 0) throw new ArgumentOutOfRangeException("page");



        //    jobs = await _repository.GetAll().Skip((page - 1) * 3).Take(3).Include(x => x.Images).ToListAsync();

        //    switch (key)
        //    {
        //        case 1:
        //            jobs = await _repository.GetAll().OrderByDescending(p => p.CategoryId).Take(6).Include(p => p.Images.Where(pi => pi.IsPrimary != null)).ToListAsync();
        //            break;

        //        case 2:
        //            jobs = await _repository.GetAll().OrderBy(p => p.Salary).Take(6).Include(p => p.Images.Where(pi => pi.IsPrimary != null)).ToListAsync();
        //            break;

        //        case 3:
        //            jobs = await _repository.GetAll().OrderByDescending(p => p.Id).Take(6).Include(p => p.Images.Where(pi => pi.IsPrimary != null)).ToListAsync();
        //            break;
        //        default:
        //            jobs = await _repository.GetAll().Take(6).Include(p => p.Images.Where(pi => pi.IsPrimary != null)).ToListAsync();
        //            break;

        //    }

        //    PaginateVM<Product> paginateVM = new PaginateVM<Product>
        //    {
        //        Items = jobs,
        //        TotalPage = (int)totalPages,
        //        CurrentPage = page
        //    };

        //    return View(paginateVM);
        //}
    }


    
}
