using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FoodOrder.Persistence;
using Microsoft.AspNetCore.Hosting;

namespace FoodOrder.WebAPI.Tests
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FoodOrderDbContext>(options =>
                options.UseInMemoryDatabase("TestingDB"));


            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<FoodOrderDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc(option => option.EnableEndpointRouting = false)
                .AddApplicationPart(Assembly.Load("FoodOrder.API"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FoodOrderDbContext context)
        {
            app.UseAuthentication();
            app.UseMiddleware<AuthenticatedTestRequestMiddleware>(); // automatikus "bejelentkezés"

            app.UseMvc();
            TestDbInitializer.Initialize(context);
        }
    }
}
