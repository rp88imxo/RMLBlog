using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PagedList.Core;
using RmlBlogMvc.Authorization;
using RmlBlogMvc.CONST_PATHS;
using RmlBlogMvc.Data.Models;
using RmlBlogMvc.LogicServices.ILogicServices;
using RmlBlogMvc.Models.UserViewModel;
using RmlBlogMvc.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RmlBlogMvc.LogicServices
{
    public class HomeLogic : IHomeLogic
    {
        private readonly ILogger<HomeLogic> loggerHome;
        private readonly IUserService userService;
        private readonly IBlogService blogService;
        private readonly IAuthorizationService authorizationService;

        public HomeLogic(ILogger<HomeLogic> loggerHome,
          IUserService userService,
          IBlogService blogService,
          IAuthorizationService authorizationService)
        {
            this.blogService = blogService;
            this.userService = userService;
            this.loggerHome = loggerHome;
            this.authorizationService = authorizationService;
        }

        public async Task<ActionResult<UserProfileViewModel>> GetUserProfile(string userid, string searchRequest, int? page, ClaimsPrincipal claimsPrincipal)
        {
            if (string.IsNullOrEmpty(userid))
            {
                return new NotFoundResult();
            }
            var user = await userService.GetUserById(userid);
            if (user==null)
            {
                return new NotFoundResult();
            }

            var authRes = await authorizationService.AuthorizeAsync(claimsPrincipal, user, Operations.Read);
            if (!authRes.Succeeded)
            {
                return CheckAuthResult(claimsPrincipal);
            }

            string sreq = searchRequest ?? string.Empty;
            var blogs = blogService.GetBlogsByUserFull(user)
                .Where(x=>x.Published 
                && (x.Title.Contains(sreq) 
                || x.Content.Contains(sreq) 
                || x.BlogCreator.FirstName.Contains(sreq)));

            int currentPage = page.GetValueOrDefault() < 1 ? 1: page.Value;

            return new UserProfileViewModel
            {
                BlogsPagedList = new StaticPagedList<Blog>
                (blogs.Skip((currentPage - 1) * ConstInfo.BLOGS_ON_PAGE_COUNT).Take(ConstInfo.BLOGS_ON_PAGE_COUNT), currentPage, ConstInfo.BLOGS_ON_PAGE_COUNT,blogs.Count()),
                CurrentPage = currentPage,
                SearchRequest = sreq,
                _User = user
            };
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
    }
}
