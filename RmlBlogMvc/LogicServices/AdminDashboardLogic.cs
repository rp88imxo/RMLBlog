using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RmlBlogMvc.Data;
using RmlBlogMvc.Data.Models;
using RmlBlogMvc.LogicServices.ILogicServices;
using RmlBlogMvc.Models.AdminDashboardViewModel;
using RmlBlogMvc.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RmlBlogMvc.LogicServices
{
    public class AdminDashboardLogic : IAdminDashboardLogic
    {
        private readonly ApplicationDbContext applicationDb;
        private readonly UserManager<User> userManager;
        private readonly IBlogService blogService;

        public AdminDashboardLogic(ApplicationDbContext applicationDbContext, UserManager<User> user, IBlogService serviceBlog)
        {
            applicationDb = applicationDbContext;
            userManager = user;
            blogService = serviceBlog;
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
    }
}
