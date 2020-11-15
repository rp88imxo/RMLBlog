using Microsoft.AspNetCore.Mvc;
using RmlBlogMvc.Models.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RmlBlogMvc.LogicServices.ILogicServices
{
    public interface IHomeLogic
    {
        Task<ActionResult<UserProfileViewModel>> GetUserProfile(string userid, string searchRequest, int? page, ClaimsPrincipal claimsPrincipal);
    }
}
