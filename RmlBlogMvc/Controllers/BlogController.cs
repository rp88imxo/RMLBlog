using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RmlBlogMvc.LogicManagers.ILogicServices;
using RmlBlogMvc.Models.BlogViewModel;
using RmlBlogMvc.Service.Interfaces;

namespace RmlBlogMvc.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly ILogger blogControllerLogger;
        private readonly IBlogLogic blogLogic;
        private readonly IBlogService blogService;

        public BlogController(ILogger<BlogController> logger, IBlogLogic blogLogic, IBlogService blogService)
        {
            blogControllerLogger = logger;
            this.blogLogic = blogLogic;
            this.blogService = blogService;
        }


        [HttpGet]
        public IActionResult Create()
        {
            var blogMV = new NewBlogViewModel();
            return View(blogMV);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewBlogViewModel blogViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await blogLogic.CreateBlog(blogViewModel, User);
            
            return RedirectToAction("Index", "AdminDashboard");
        }

        public async Task<IActionResult> EditBlog(int BlogId)
        {
            var EditBlogActionResult = await blogLogic.EditBlogViewModel(BlogId, User);

            if (EditBlogActionResult.Result==null)
            {
                return View(EditBlogActionResult.Value);
            }

            return EditBlogActionResult.Result;
        }

        [HttpPost]
        public async Task<IActionResult> Post(BlogViewModel BlogViewModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View("NotFoundPage");
            //}
            var CreatePostActionResult = await blogLogic.CreatePost(BlogViewModel, User);

            if (CreatePostActionResult.Result == null)
            {

                return RedirectToAction("Blog","Home", new {blogid= BlogViewModel.Blog.Id });
            }

            return CreatePostActionResult.Result;
        }

        public async Task<IActionResult> UpdateBlog(EditBlogViewModel editBlogViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("NotFoundPage");
            }
            var UpdateBlogActionResult = await blogLogic.UpdateBlogViewModel(editBlogViewModel, User);

            if (UpdateBlogActionResult.Result == null)
            {
                return RedirectToAction("Index", "AdminDashboard");
            }

            return UpdateBlogActionResult.Result;
        }
        public async Task<IActionResult> DeleteBlog(int BlogId)
        {
            var DeleteBlogActionResult = await blogLogic.DeleteBlogViewModel(BlogId, User);

            if (DeleteBlogActionResult.Result == null)
            {
                return RedirectToAction("Index", "AdminDashboard");
            }

            return DeleteBlogActionResult.Result;
        }
    }
}
