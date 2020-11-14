using Microsoft.AspNetCore.Mvc;
using RmlBlogMvc.Data.Models;
using RmlBlogMvc.Models.BlogsHomePageViewModel;
using RmlBlogMvc.Models.BlogViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RmlBlogMvc.LogicManagers.ILogicServices
{
   public interface IBlogLogic
    {
        Task<Blog> CreateBlog(NewBlogViewModel w, ClaimsPrincipal w1);
        Task<ActionResult<EditBlogViewModel>> EditBlogViewModel(int? id, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditBlogViewModel>> UpdateBlogViewModel(EditBlogViewModel editBlogViewModel, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<DeleteBlogViewModel>> DeleteBlogViewModel(int? id, ClaimsPrincipal claimsPrincipal);
        BlogsHomeViewModel GetBlogsHomeViewModel(string searchRequest, int? pageRequest);
    }
}
