﻿using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Repositories.Generic;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.Utilities;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Persistence.Implementations.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FinalProjectFb.Persistence.Implementations.Services
{
    internal class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly ICityRepository _cityRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserService _user;

        public CompanyService(ICompanyRepository repository,ICityRepository cityRepository,IWebHostEnvironment env,IHttpContextAccessor accessor,IUserService user)
        {
            _repository = repository;
            _cityRepository = cityRepository;
            _env = env;
            _accessor = accessor;
            _user = user;
        }
        public async Task<bool> GetConfirmationFormAsync(ConfirmationFormVM vm, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;

            if (await _repository.IsExistAsync(c => c.Name == vm.Name))
            {
                modelstate.AddModelError("Name", "This Company already exists");
                return false;
            }

            AppUser User = await _user.GetUser(_accessor.HttpContext.User.Identity.Name);

            Image photo = new Image
            {
                IsPrimary = true,
                Url = await vm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "icon")
            };

            Company company = new Company
            {
                AppUserId = User.Id,
                CreatedBy = User.UserName,
                CreatedAt = DateTime.UtcNow,
                Name = vm.Name,
                WebsiteLink = vm.WebsiteLink,
                FacebookLink = vm.FacebookLink,
                TwitterLink = vm.TwitterLink,
                GmailLink = vm.GmailLink,
                InstagramLink = vm.InstagramLink,
                IsDeleted = null,
                CompanyCities = new List<CompanyCity>(),
                Description = vm.Description,
                Images = new List<Image> { photo },
            };

            if (vm.CityIds != null)
            {
                foreach (var item in vm.CityIds)
                {
                    if (!await _cityRepository.IsExistAsync(c => c.Id == item))
                    {
                        modelstate.AddModelError(String.Empty, "This city does not exist");
                        return false;
                    }
                    company.CompanyCities.Add(new CompanyCity
                    {
                        CityId = item
                    });
                }
            }

            await _repository.AddAsync(company);
            await _repository.SaveChangesAsync();
            return true;
        }
        public async Task ReverseDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Company existed = await _repository.GetByIdAsync(id, isDeleted: true);
            if (existed == null) throw new Exception("Not Found");
            _repository.ReverseDelete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Company existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            _repository.SoftDelete(existed);
            await _repository.SaveChangesAsync();
        }

        //public async void ReverseDeleteCompany(int companyId)
        //{
        //    // İlgili şirketi al
        //    Company company = await _repository.GetByIdAsync(companyId);

        //    if (company != null)
        //    {
        //        // Şirketin silinmiş bayrağını geri çevir
        //        company.IsDeleted = false;

        //        // Depoya güncelleme yap
        //        _repository.Update(company);
        //    }
        //    else
        //    {
        //        // Şirket bulunamadı, gerekirse bir hata işle
        //       throw new Exception("Şirket bulunamadı");
        //    }
        //}
        //public async Task<bool> DeletedFormStatus(int id)
        //{
        //    if (id < 1) throw new ArgumentOutOfRangeException("id");
        //    Company company = await _repository.GetByIdAsync(id);
        //    if (company == null) throw new Exception("Not found");
        //    _repository.SoftDelete(company);
        //    return true;
        //}
        public async Task<ConfirmationFormVM> GetCitiesForConfirmationFormAsync(ConfirmationFormVM confirmationFormVM)
        {
            try
            {
                var cityList = _cityRepository.GetAll().ToList(); 
                confirmationFormVM.Cities = cityList;
                return confirmationFormVM;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error in GetCitiesForConfirmationFormAsync: {ex.Message}");
                return confirmationFormVM;
            }
        }


        public async Task<PaginateVM<Company>> GetAllAsync(int page = 1, int take = 10)
        {
            ICollection<Company> companies = await _repository.GetPagination(skip: (page - 1) * take, take: take).ToListAsync();
            int count = await _repository.GetAll().CountAsync();
            double totalpage = Math.Ceiling((double)count / take);
            PaginateVM<Company> vm = new PaginateVM<Company>
            {
                Items = companies,
                CurrentPage = page,
                TotalPage = totalpage
            };
            return vm;
        }
        public async Task<CompanyDetailVM> DetailAsync(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException("id");

            Company company = await _repository.GetByIdAsync(id, includes: new string[] {"CompanyCities","Images" });


            CompanyItemVM companyItemVM = new CompanyItemVM
            {
              
                Name = company.Name,               
                Images = company.Images,             
                FacebookLink = company.FacebookLink,
                TwitterLink = company.TwitterLink,
                InstagramLink = company.InstagramLink,
                GmailLink = company.GmailLink,
                WebsiteLink = company.WebsiteLink,
                Description = company.Description,
                CompanyCities= company.CompanyCities,
                CompanyId=company.Id
            };
            if (company is null) throw new Exception("Not found");           

            CompanyDetailVM detailVM = new CompanyDetailVM
            {
               
                Company = companyItemVM
            };
            return detailVM;
        }

        


        //public async Task<ConfirmationFormVM> GetCitiesForConfirmationFormAsync(ConfirmationFormVM confirmationFormVM)
        //{
        //    confirmationFormVM.Cities=await _cityRepository.GetAll().ToListAsync();
        //    return confirmationFormVM;
        //}



    }
}
