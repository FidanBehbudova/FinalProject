using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Repositories.Generic;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.Utilities;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Persistence.DAL;
using FinalProjectFb.Persistence.Implementations.Repositories;
using FinalProjectFb.Persistence.Implementations.Repositories.Generic;
using FinalProjectFb.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FinalProjectFb.Persistence.Implementations.Services
{
    internal class JobService : IJobService
    {
        private readonly IJobRepository _repository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserService _user;
        private readonly IWebHostEnvironment _env;
        private readonly ICompanyRepository _company;
        private readonly ICategoryRepository _category;

        public JobService(IJobRepository repository,IHttpContextAccessor accessor,IUserService user,IWebHostEnvironment env,ICompanyRepository company,ICategoryRepository category)
        {
            _repository = repository;
            _accessor = accessor;
            _user = user;
            _env = env;
            _company = company;
            _category = category;
        }
        public async Task<bool> Create(CreateJobVM createJobVM, ModelStateDictionary modelstate)
        {

            if (!modelstate.IsValid) return false;
          
            if (createJobVM.CategoryId is not null)
            {
                if (!await _category.IsExist(x => x.Id == createJobVM.CategoryId))
                {
                    modelstate.AddModelError("CategoryId", "You dont have this Category");
                    return false;
                }

            }

            string username = "";
            if (_accessor.HttpContext.User.Identity != null)
            {
                username = _accessor.HttpContext.User.Identity.Name;
            }
            AppUser User = await _user.GetUser(username);

            Company company = await _company.GetByExpressionAsync(x => x.AppUserId == User.Id, isDeleted: false, includes: new string[] { nameof(Company.Jobs) });
            if (company.Jobs.Where(x => x.Name == createJobVM.Name && x.CompanyId == company.Id).Count() >= 1)
            {
                modelstate.AddModelError("Name", "You have this job in your Jobs");
                return false;
            }
            if (createJobVM.Photo != null)
            {
                if (!createJobVM.Photo.ValidateType("image/"))
                {
                    modelstate.AddModelError("Photo", "File type does not match. Please upload a valid image.");
                    return false;
                }
                if (!createJobVM.Photo.ValidateSize(600))
                {
                    modelstate.AddModelError("Photo", "File size should not be larger than 2MB.");
                    return false;
                }
            }

            Image photo = new Image
            {
                IsPrimary = true,
                Url = await createJobVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "icon")
            };

            Job job = new Job
            {
                Name = createJobVM.Name,
                Requirement = createJobVM.Requirement,              
                JobNature = createJobVM.JobNature,
                Experience = createJobVM.Experience,
                Deadline = createJobVM.Deadline,
                Salary = createJobVM.Salary,
                Vacancy = createJobVM.Vacancy,
                AppUserId = User.Id,
                CreatedBy = User.UserName,
                CategoryId =createJobVM.CategoryId,              
                CreatedAt = DateTime.UtcNow,
                //CompanyId = createJobVM.CompanyId,
                Images = new List<Image> { photo }
            };
            job.CompanyId = company.Id;

            
            await _repository.AddAsync(job);
            await _repository.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> Create(CreateJobVM createJobVM, ModelStateDictionary modelstate)
        //{
        //    if (!modelstate.IsValid) return false;


        //    AppUser User = await _user.GetUser(_accessor.HttpContext.User.Identity.Name);


        //    if (createJobVM.Photo != null)
        //    {
        //        if (!createJobVM.Photo.ValidateType("image/"))
        //        {
        //            modelstate.AddModelError("Photo", "File type does not match. Please upload a valid image.");
        //            return false;
        //        }
        //        if (!createJobVM.Photo.ValidateSize(600))
        //        {
        //            modelstate.AddModelError("Photo", "File size should not be larger than 2MB.");
        //            return false;
        //        }
        //    }


        //    Image photo = new Image()
        //    {
        //        IsPrimary = true,
        //        Url = await createJobVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img"),              

        //    };


        //    Job job = new Job
        //    {
        //        Name = createJobVM.Name,
        //        JobNature = createJobVM.JobNature,
        //        Experience = createJobVM.Experience,
        //        Deadline = createJobVM.Deadline,
        //        Salary = createJobVM.Salary,
        //        Vacancy = createJobVM.Vacancy,
        //        AppUserId = User.Id,
        //        CreatedBy = User.UserName,
        //        CategoryId = (int)createJobVM.CategoryId,              
        //        CreatedAt = DateTime.UtcNow,
        //        CompanyId = createJobVM.CompanyId,
        //        Images = new List<Image> { photo }

        //    };

        //    await _repository.AddAsync(job);
        //    await _repository.SaveChangesAsync();
        //    return true;

        //}

        public async Task<CreateJobVM> CreatedAsync(CreateJobVM vm)
        {

            vm.Categories = await _category.GetAll().ToListAsync();
           
            return vm;
        }

        public async Task<JobDetailVM> DetailAsync(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException("id");

            Job job = await _repository.GetByIdAsync(id, includes: new string[] {"Company", "Company.CompanyCities", "Company.CompanyCities.City", "Category","Images" });

           
            JobItemVM jobItemVM = new JobItemVM
            {
                Vacancy = job.Vacancy,
                JobNature = job.JobNature,
                Requirement = job.Requirement,
                
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
