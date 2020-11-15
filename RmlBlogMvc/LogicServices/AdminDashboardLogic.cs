using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RmlBlogMvc.Data;
using RmlBlogMvc.Data.Models;
using RmlBlogMvc.LogicServices.ILogicServices;
using RmlBlogMvc.Models.AdminDashboardViewModel;
using RmlBlogMvc.Models.UserViewModel;
using RmlBlogMvc.Service.Interfaces;
using RmlBlogMvc.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RmlBlogMvc.LogicServices
{
    public class AdminDashboardLogic : IAdminDashboardLogic
    {
        private readonly ApplicationDbContext applicationDb;
        private readonly UserManager<User> userManager;
        private readonly ILogger<AdminDashboardLogic> adminDashboardLogger;
        private readonly IBlogService blogService;
        private readonly IUserService userService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration configuration;


        public AdminDashboardLogic(ApplicationDbContext applicationDbContext, 
            UserManager<User> user, 
            IBlogService serviceBlog,
            IUserService userService,
            IWebHostEnvironment webHostEnv,
            IConfiguration config,
            ILogger<AdminDashboardLogic> adminDashboardLogger
            )
        {
            applicationDb = applicationDbContext;
            userManager = user;
            blogService = serviceBlog;

            this.userService = userService;
            this.webHostEnvironment = webHostEnv;
            this.configuration = config;
            this.adminDashboardLogger = adminDashboardLogger;
        }

        public async Task<DashboardViewModel> GetDashboard(ClaimsPrincipal claimsPrincipal)
        {
            var currentUser = await userManager.GetUserAsync(claimsPrincipal);
            var blogs = await blogService.GetBlogs(currentUser).ToListAsync();

            return new DashboardViewModel()
            {
                Blogs = blogs
            };
        }

        public async Task UpdateUserAboutInfo(ClaimsPrincipal claimsPrincipal, UserAboutInfoViewModel userAboutInfoViewModel)
        {
            var currentUser = await userManager.GetUserAsync(claimsPrincipal);
            UserUpdateInfo(currentUser, userAboutInfoViewModel);
            await userService.Update(currentUser);

            if (userAboutInfoViewModel.AboutImage != null)
            {
                string blogImagePath = Path.Combine(
                    webHostEnvironment.WebRootPath,
                    configuration["ServerFilesFolder"],
                    configuration["UserFilesFolder"],
                    "UserImages",
                    $"{currentUser.Id}",
                    "UserImage.jpg"
                    );

                try
                {
                    RmlUtils.EnsureFolderCreated(blogImagePath);
                    using (var fs = new FileStream(blogImagePath, FileMode.Create))
                    {
                        await userAboutInfoViewModel.AboutImage.CopyToAsync(fs);
                    }
                }
                catch (Exception ex)
                {
                    adminDashboardLogger.LogError(ex.Message);
                    throw ex;
                }
               

            }
        }

        public async Task<UserAboutInfoViewModel> GetUserAboutInfoViewModel(ClaimsPrincipal claimsPrincipal)
        {
            var currentUser = await userManager.GetUserAsync(claimsPrincipal);
            return new UserAboutInfoViewModel
            {
                AboutInfoContent = currentUser.UserAboutInfoContent,
                AboutInfoHeader = currentUser.UserAboutInfoHeader,
                AllowedToOthers = currentUser.UserAboutInfoAllowedToOthers
            };
        }

        private void UserUpdateInfo(User user, UserAboutInfoViewModel userAboutInfoViewModel)
        {
            user.UserAboutInfoContent = userAboutInfoViewModel.AboutInfoContent;
            user.UserAboutInfoHeader = userAboutInfoViewModel.AboutInfoHeader;
            user.UserAboutInfoAllowedToOthers = userAboutInfoViewModel.AllowedToOthers;
        }
    }
}
