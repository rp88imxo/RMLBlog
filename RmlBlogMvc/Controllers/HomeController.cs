using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RmlBlogMvc.LogicManagers.ILogicServices;
using RmlBlogMvc.Models;
using RmlBlogMvc.Models.BlogViewModel;

namespace RmlBlogMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> homeLogger;
        private readonly IBlogLogic blogLogic;

        public HomeController(ILogger<HomeController> logger, IBlogLogic blogLogic)
        {
            homeLogger = logger;
            this.blogLogic = blogLogic;
        }

        
        public IActionResult Index(string searchRequest, int? page)
        {
            var blogsHomeVM = blogLogic.GetBlogsHomeViewModel(searchRequest, page);
            return View(blogsHomeVM);
        }

        // Show a blog page filled with content
        public async Task<IActionResult> Blog(int? blogid)
        {
            if (!ModelState.IsValid)
            {
                homeLogger.LogInformation("Valid blogViewModel!");
                return BadRequest();
            }

            var blogVM = await blogLogic.GetBlogViewModel(blogid, User);
            if (blogVM.Result==null)
            {
                return View(blogVM.Value);
            }

            return blogVM.Result;
        }
        public IActionResult NotFoundPage()
        {
            return View("NotFoundPage");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
