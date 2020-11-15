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
using Microsoft.Extensions.Logging;
using RmlBlogMvc.Models.BlogsHomePageViewModel;
using RmlBlogMvc.CONST_PATHS;
using PagedList.Core;
using RmlBlogMvc.Service;

namespace RmlBlogMvc.LogicServices
{
    public class BlogLogic : IBlogLogic
    {
        private readonly UserManager<User> userManager;
        private readonly IBlogService blogService;
        private readonly IPostService postService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration configuration;
        private readonly IAuthorizationService authorizationService;
        private readonly ILogger<BlogLogic> blogLogicLogger;

        public BlogLogic(UserManager<User> userManager, 
            IBlogService blogService, 
            IAuthorizationService authorizationService, 
            IWebHostEnvironment env, 
            IConfiguration configuration,
            ILogger<BlogLogic> blogLogicLogger,
            IPostService postService
            )
        {
            this.userManager = userManager;
            this.blogService = blogService;
            webHostEnvironment = env;
            this.configuration = configuration;
            this.authorizationService = authorizationService;
            this.blogLogicLogger = blogLogicLogger;
            this.postService = postService;
        }

        public async Task<ActionResult<Post>> CreatePost(BlogViewModel blogViewModel, ClaimsPrincipal claimsPrincipal)
        {
            if (blogViewModel.Blog==null )
            {
                return new BadRequestResult();
            }

            var blog = await blogService.GetBlogByIdWithAllReplies(blogViewModel.Blog.Id);
            if (blog ==null)
            {
                return new NotFoundResult();
            }

            var post = blogViewModel.Post;
            await UpdatePostFields(post, blog, claimsPrincipal);
            if (post.RelatedPost != null)
            {
                post.RelatedPost = await postService.GetPostById(post.RelatedPost.Id);
            }

            await postService.Add(post);
            return post;
        }
        private async Task UpdatePostFields(Post post,Blog blog, ClaimsPrincipal claimsPrincipal)
        {
            post.PostCreator = await userManager.GetUserAsync(claimsPrincipal);
            post.Blog = blog;
            post.CreationTime = DateTime.Now;
        }
        public BlogsHomeViewModel GetBlogsHomeViewModel(string searchRequest, int? pageRequest)
        {
            int currentPage = pageRequest == null || pageRequest < 1 ? 1 : (int)pageRequest;
            
            var blogs = blogService.GetBlogsBySearchRequest(searchRequest ?? string.Empty)
                .Where(x=>x.Published==true);
               

            return new BlogsHomeViewModel
            {
                BlogsPagedList = new StaticPagedList<Blog>
                (
                 blogs.Skip((currentPage - 1) * ConstInfo.BLOGS_ON_PAGE_COUNT)
                .Take(ConstInfo.BLOGS_ON_PAGE_COUNT), currentPage, ConstInfo.BLOGS_ON_PAGE_COUNT, blogs.Count()),
                SearchRequest = searchRequest,
                CurrentPage = currentPage
            };
        }

        public async Task<ActionResult<BlogViewModel>> GetBlogViewModel(int? blogid, ClaimsPrincipal claimsPrincipal)
        {
            if (blogid == null)
            {
                return new BadRequestResult();
            }
            var blog = await blogService.GetBlogByIdFull(blogid.Value);

            if (blog==null)
            {
                return new NotFoundResult();
            }

            /* Check if we have permissions to view an unpublished blog
             * blog can be viewed only by Blog Creator IF it unpublished yet
            */
            var authRes = await authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Read);
            if (!authRes.Succeeded)
            {
                return CheckAuthResult(claimsPrincipal);
            }




            return new BlogViewModel
            {
                Blog = blog
            };

        }
        public async Task<ActionResult<EditBlogViewModel>> UpdateBlogViewModel(EditBlogViewModel editBlogViewModel, ClaimsPrincipal claimsPrincipal)
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
                return CheckAuthResult(claimsPrincipal);
            }

            

            if (editBlogViewModel.BlogHeaderImage != null)
            {
                string blogImagePath = Path.Combine(
                    webHostEnvironment.WebRootPath,
                    configuration["ServerFilesFolder"],
                    configuration["BlogFilesFolder"],
                    "BlogImages",
                    $"{blog.Id}",
                    "BlogImage.jpg"
                    );

                try 
                {
                    RmlUtils.EnsureFolderCreated(blogImagePath);
                    using (var fs = new FileStream(blogImagePath, FileMode.Create))
                    {
                        await editBlogViewModel.BlogHeaderImage.CopyToAsync(fs);
                    }
                }
                catch (Exception ex)
                {
                    blogLogicLogger.LogError(ex.Message);
                    throw ex;
                }    
            }
            
            UpdateBlogFields(blog, editBlogViewModel);
            await blogService.Update(blog); //actuall update to database
            blogLogicLogger.LogInformation($"User {claimsPrincipal.Identity.Name} edited blog \"{blog.Title}\" ");
            return editBlogViewModel;

        }


        public async Task<ActionResult<EditBlogViewModel>> EditBlogViewModel(int? id, ClaimsPrincipal claimsPrincipal)
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
            
            // Check if we have permissions to perfom an Edit Blog (need to be the same user who created a blog)
            var authRes = await authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);
            if (!authRes.Succeeded)
            {
                return CheckAuthResult(claimsPrincipal);
            }

            
            return new EditBlogViewModel
            {
                Blog = blog
            };
        }

        public async Task<ActionResult<DeleteBlogViewModel>> DeleteBlogViewModel(int? id, ClaimsPrincipal claimsPrincipal)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            var blog = await blogService.GetBlogById((int)id);
            if (blog == null)
            {
                return new NotFoundResult();
            }

            // Check if we have permissions to perfom an Edit Blog (need to be the same user who created a blog)
            var authRes = await authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);
            if (!authRes.Succeeded)
            {
                return CheckAuthResult(claimsPrincipal);
            }

            // Deleting associated with that blog image
            DeleteBlogImageFromServer((int)id);

            // Actuall delete form database
            // TODO: check if it should be wrapped in trycatch
            var res = await blogService.Delete(blog);



            blogLogicLogger.LogInformation($"User {claimsPrincipal.Identity.Name} deleted blog \"{blog.Title}\" ");
            return new DeleteBlogViewModel
            {
                Blog = blog
            };
        }

        public async Task<Blog> CreateBlog(NewBlogViewModel newBlogView, ClaimsPrincipal claimsPrincipal)
        {
            Blog blog = newBlogView.Blog;

            blog.CreationTime = DateTime.Now;
            blog.EditedTime = blog.CreationTime;
            blog.isAllowedToPost = true; // TODO: Add approvance functionality to the blogs - post only if its approved by user with Admin priviliges

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

            blogLogicLogger.LogInformation($"User {claimsPrincipal.Identity.Name} created new blog \"{blog.Title}\" ");
            return blog;
        }
        private void UpdateBlogFields(Blog origin, EditBlogViewModel NewBlogVM)
        {
            origin.Content = NewBlogVM.Blog.Content;
            origin.Title = NewBlogVM.Blog.Title;
            origin.EditedTime = DateTime.Now;
            origin.Published = NewBlogVM.Blog.Published;
        }
        private ActionResult CheckAuthResult(ClaimsPrincipal claimsPrincipal)
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

        private int DeleteBlogImageFromServer(int blogId)
        {
            string blogImagePath = Path.Combine(
                    webHostEnvironment.WebRootPath,
                    configuration["ServerFilesFolder"],
                    configuration["BlogFilesFolder"],
                    "BlogImages",
                    $"{blogId}",
                    "BlogImage.jpg"
                    );

            if (Directory.Exists(Path.GetDirectoryName(blogImagePath)))
            {
                Directory.Delete(Path.GetDirectoryName(blogImagePath), true);
                return 0;
            }
            return -1;
        }
    }
}
