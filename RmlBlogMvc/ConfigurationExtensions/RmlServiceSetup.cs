using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RmlBlogMvc.Data;
using RmlBlogMvc.Data.Models;
using RmlBlogMvc.LogicManagers.ILogicServices;
using RmlBlogMvc.LogicServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RmlBlogMvc.Authorization;
using RmlBlogMvc.Service;
using RmlBlogMvc.Service.Interfaces;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using RmlBlogMvc.LogicServices.ILogicServices;
using Microsoft.AspNetCore.Authorization;
using RmlBlogMvc.AdditionalServices.interfaces;
using RmlBlogMvc.AdditionalServices;

namespace RmlBlogMvc.ConfigurationExtensions
{
    public static class RmlServiceSetup
    {
        public static void AddDefaultServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddSingleton<IFileProvider>
                (new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), env.WebRootPath)));
            
        }

        public static void AddLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IBlogLogic, BlogLogic>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IAdminDashboardLogic, AdminDashboardLogic>();
            services.AddScoped<IGetRandomService, GetRandomNumberService>();
        }

        public static void AddAuthServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, BlogAuthHandler>();
        }
    }
}
