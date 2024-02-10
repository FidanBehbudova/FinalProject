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

            var citiesVM = await GetCitiesForConfirmationFormAsync();

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
                    if (!citiesVM.Cities.Any(c => c.Id == item))
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



        public async Task<ConfirmationFormVM> GetCitiesForConfirmationFormAsync()
        {
            var cities = await _cityRepository.GetAll().ToListAsync();

            return new ConfirmationFormVM
            {
                Cities = cities
            };
        }



    }
}
