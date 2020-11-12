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
    public class BlogAuthHandler : AuthorizationHandler<OperationAuthorizationRequirement, Blog>
    {
        private readonly UserManager<User> userManager;
        public BlogAuthHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Blog resource)
        {
            User currentUser = await userManager.GetUserAsync(context.User);

            if (currentUser == resource.BlogCreator && (requirement.Name == Operations.Delete.Name || requirement.Name == Operations.Update.Name))
            {
                context.Succeed(requirement);
            }
        }
    }
}
