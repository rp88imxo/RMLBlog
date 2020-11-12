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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RmlBlogMvc.Authorization;

namespace RmlBlogMvc.LogicServices
{
    public class BlogLogic : IBlogLogic
    {
        private readonly UserManager<User> userManager;
        private readonly IBlogService blogService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration configuration;
        private readonly IAuthorizationService authorizationService;

        public BlogLogic(UserManager<User> userManager, IBlogService blogService, IAuthorizationService authorizationService, IWebHostEnvironment env, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.blogService = blogService;
            webHostEnvironment = env;
            this.configuration = configuration;
            this.authorizationService = authorizationService;
        }

        public async Task<ActionResult<EditBlogViewModel>> UpdateBlog(EditBlogViewModel editBlogViewModel, ClaimsPrincipal claimsPrincipal)
        {
            var blog = await blogService.GetBlogById(editBlogViewModel.Blog.Id);
            if (blog == null)
            {
                return new NotFoundResult();
            }

            // Check if we have permissions to perfom an Edit Blog (need to be the same user who created a blog)
            var authRes = await authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);
            if (!authRes.Succeeded)
            {
                if (claimsPrincipal.Identity.IsAuthenticated)
                {
                    return new ForbidResult();
                }
                else
                {
                    return new ChallengeResult();
                }
            }



        }


        public async Task<ActionResult<EditBlogViewModel>> CreateEditBlogViewModel(int? id, ClaimsPrincipal claimsPrincipal)
        {
            if (id==null)
            {
                return new BadRequestResult();
            }

           var blog = await blogService.GetBlogById((int)id);
            if (blog==null)
            {
                return new NotFoundResult();
            }
            
            // Check if we have permissions to perfom an Edit Blog (need to be the same user who created a blog
            var authRes = await authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);
            if (!authRes.Succeeded)
            {
                if (claimsPrincipal.Identity.IsAuthenticated)
                {
                    return new ForbidResult();
                }
                else
                {
                    return new ChallengeResult();
                }
            }

            return new EditBlogViewModel
            {
                Blog = blog
            };
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
        private void UpdateBlog(Blog origin, EditBlogViewModel NewBlogVM)
        {
            origin.Content = NewBlogVM.Blog.Content;
            origin.Title = NewBlogVM.Blog.Title;
            origin.EditedTime = DateTime.Now;
        }
    }
}
