using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyPortfolio.Core.Interfaces;
using MyPortfolio.Infrastructure;
using MyPortfolio.Infrastructure.UnitOfWork;

namespace MyPortfolio.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //configuring mvc
            services.AddControllersWithViews();

            //Gets the designated connectionstring from within the appsettings.json file
            //Reference to "infrastructure" project MUST be added to this project
            // VIP~!!!! I have run into some errors after manually installing ef 3.1.7 through NPM
            //SO, I used this instead in the PMC
            //"dotnet tool install --global dotnet-ef --version 3.1.7"
            //Also make sure you remove <PrivateAssets>all</PrivateAssets> from the destination infrastructure proj file
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("AbdoZPortfolioDB"));
            });

            //registers the UOW services
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //using static files in wwwroot
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //do NOT leave leading or trailing spaces before or after the =
                endpoints.MapControllerRoute("defaultRoute", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
