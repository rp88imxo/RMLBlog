using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using RmlBlogMvc.Data.Models;
using RmlBlogMvc.LogicManagers.ILogicServices;
using RmlBlogMvc.Models.BlogViewModel;
using RmlBlogMvc.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using System.IO;
using Microsoft.Extensions.Configuration;

using RmlBlogMvc.utils;

namespace RmlBlogMvc.LogicServices
{
    public class BlogLogic : IBlogLogic
    {
        private readonly UserManager<User> userManager;
        private readonly IBlogService blogService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration configuration;

        public BlogLogic(UserManager<User> userManager, IBlogService blogService, IWebHostEnvironment env, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.blogService = blogService;
            webHostEnvironment = env;
            this.configuration = configuration;
        }


        public async Task<Blog> CreateBlog(NewBlogViewModel newBlogView, ClaimsPrincipal claimsPrincipal)
        {
            Blog blog = newBlogView.Blog;

            blog.CreationTime = DateTime.Now;
            blog.EditedTime = blog.CreationTime;

            blog.BlogCreator = await userManager.GetUserAsync(claimsPrincipal);

            blog = await blogService.Add(blog);

            string wwwroot = webHostEnvironment.WebRootPath;
            string blogImagePath = Path.Combine(
                wwwroot,
                configuration["ServerFilesFolder"],
                configuration["BlogFilesFolder"],
                "BlogImages",
                $"{blog.Id}",
                "BlogImage.jpg"
                );
            
            
            
            RmlUtils.EnsureFolderCreated(blogImagePath);
            using (var fs = new FileStream(blogImagePath, FileMode.Create))
            {
                await newBlogView.BlogHeaderImage.CopyToAsync(fs);
            }

            return blog;
        }
    }
}
