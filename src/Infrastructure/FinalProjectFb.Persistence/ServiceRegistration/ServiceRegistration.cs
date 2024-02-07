using FinalProjectFb.Persistence.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Persistence.Implementations.Repositories;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Persistence.Implementations.Services;
using FluentValidation.AspNetCore;
using FinalProjectFb.Application.Validators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FinalProjectFb.Application.MappingProfiles;
using Microsoft.IdentityModel.Tokens;

namespace FinalProjectFb.Persistence.ServiceRegistration
{
	public static class ServiceRegistration
	{
		public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Default")));

			services.AddIdentity<AppUser, IdentityRole>(opt =>
			{
				opt.Password.RequireNonAlphanumeric = false;
				opt.Password.RequiredLength = 8;

				opt.User.RequireUniqueEmail = true;

				opt.Lockout.MaxFailedAccessAttempts = 5;
				opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				opt.Lockout.AllowedForNewUsers = true;
			}).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
			
			
			services.AddScoped<IHomeService, HomeService>();
			services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICityService, CityService>();




            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
			services.AddScoped<INewsRepository, NewsRepository>();
			services.AddScoped<ICompanyRepository, CompanyRepository>();

			return services;
		}
	}
}
