using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using RmlBlogMvc.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RmlBlogMvc.Authorization
{
    public class UserAuthHandler : AuthorizationHandler<OperationAuthorizationRequirement, User>
    {
        private readonly UserManager<User> UserManager;
        public UserAuthHandler(UserManager<User> UserManager)
        {
            this.UserManager = UserManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, User resource)
        {
            var currentUser = await UserManager.GetUserAsync(context.User);
            if ((currentUser.Id==resource.Id || (resource.UserAboutInfoAllowedToOthers)) && requirement.Name==Operations.Read.Name)
            {
                context.Succeed(requirement);
            }
            
           
        }
    }
}
