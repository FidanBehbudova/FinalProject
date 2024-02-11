using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Repositories.Generic;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.Utilities;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Persistence.Implementations.Services
{
    internal class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly ICityRepository _cityRepository;
        private readonly IWebHostEnvironment _env;

        public CompanyService(ICompanyRepository repository,ICityRepository cityRepository,IWebHostEnvironment env)
        {
            _repository = repository;
            _cityRepository = cityRepository;
            _env = env;
        }
        public async Task<bool> GetConfirmationFormAsync(ConfirmationFormVM vm, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;

            if (await _repository.IsExistAsync(c => c.Name == vm.Name))
            {
                modelstate.AddModelError("Name", "This Company already exists");
                return false;
            }

          

            Image photo = new Image
            {
                IsPrimary = true,
                Url = await vm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "icon")
            };

            Company company = new Company
            {
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

        public async Task<ConfirmationFormVM> GetCitiesForConfirmationFormAsync(ConfirmationFormVM confirmationFormVM)
        {
            try
            {
                var cityList = _cityRepository.GetAll().ToList(); // Bu satırı ekleyin ve değeri kontrol edin
                confirmationFormVM.Cities = cityList;
                return confirmationFormVM;
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama veya hata mesajını gösterme
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
