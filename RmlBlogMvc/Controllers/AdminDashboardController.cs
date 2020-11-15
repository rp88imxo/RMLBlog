using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using RmlBlogMvc.Data.Models;
using RmlBlogMvc.LogicServices.ILogicServices;
using RmlBlogMvc.Models.AdminDashboardViewModel;
using RmlBlogMvc.Models.UserViewModel;

namespace RmlBlogMvc.Controllers
{
    [Authorize]
    public class AdminDashboardController : Controller
    {
        private readonly ILogger<AdminDashboardController> adminLogger;
        private readonly IAdminDashboardLogic adminDashboardService;
        private readonly SignInManager<User> signInManager;


        public AdminDashboardController(
            ILogger<AdminDashboardController> logger,
            IAdminDashboardLogic adminDashboardLogic,
            SignInManager<User> signInManager
            )
        {

            adminLogger = logger;
            adminDashboardService = adminDashboardLogic;
            this.signInManager = signInManager;
        }


        public async Task<IActionResult> Index()
        {
            if (!signInManager.IsSignedIn(User))
            {
                adminLogger.LogWarning($"Unautorized attempt to get {HttpContext.Request.Path.Value} from user {User.Identity.Name}");
                return Unauthorized();
            }

            DashboardViewModel blogs = await adminDashboardService.GetDashboard(User);

            adminLogger.LogInformation($"User {User.Identity.Name} connected to {HttpContext.Request.Path.Value}");
            return View(blogs);
        }

        [HttpGet]
        public async Task<IActionResult> UserAbout()
        {
            var UserAboutInfoVM = await adminDashboardService.GetUserAboutInfoViewModel(User);
            return View(UserAboutInfoVM);
        }

        [HttpPost]
        public async Task<IActionResult> UserAbout(UserAboutInfoViewModel userAboutInfoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await adminDashboardService.UpdateUserAboutInfo(User, userAboutInfoViewModel);
            return RedirectToAction("UserAbout");
        }
       
    }
}
