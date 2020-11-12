using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using RmlBlogMvc.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RmlBlogMvc.ConfigurationExtensions;

namespace RmlBlogMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {

            var b = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .AddJsonFile("configs/rmlconfig.json");
            Configuration = b.Build();
            env = webHostEnvironment;
        }

        public readonly IConfiguration Configuration;
        public readonly IWebHostEnvironment env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDefaultServices(Configuration, env);
            
            // Custom services to serve some validations and etc
            services.AddLogicServices();

            // Custom auth service to check operations
            services.AddAuthServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaultSetup(env);

            
        }
    }
}
