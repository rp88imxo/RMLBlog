using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RmlBlogMvc.LogicManagers.ILogicServices;
using RmlBlogMvc.Models.BlogViewModel;
using RmlBlogMvc.Service.Interfaces;

namespace RmlBlogMvc.Controllers
{
    
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
                return View();
            }
            var blog = await blogLogic.CreateBlog(blogViewModel, User);
            
            return View("BlogCreated", blogViewModel);
        }

        public async Task<IActionResult> EditBlog(int BlogId)
        {
            var EditBlogActionResult = await blogLogic.CreateEditBlogViewModel(BlogId, User);

            if (EditBlogActionResult==null)
            {
                return View(EditBlogActionResult.Value);
            }

            return EditBlogActionResult.Result;
           
        }

    }
}
