using FinalProjectFb.Persistence.DAL;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectFb.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			Environment = env;
		}

		public IConfiguration Configuration { get; }
		public IWebHostEnvironment Environment { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			// Servisleri burada kaydedin, örneğin:
			services.AddMvc();
			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			// AutoMapper servisini ekleyin
			services.AddAutoMapper(typeof(Startup));

			// Diğer servis kayıtları
		}

		public void Configure(IApplicationBuilder app)
		{
			// Middleware'leri burada ekleyin
			if (Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			// Route'ları ve statik dosyaları burada tanımlayın
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}

}
