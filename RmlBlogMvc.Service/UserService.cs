using Microsoft.EntityFrameworkCore;
using RmlBlogMvc.Data;
using RmlBlogMvc.Data.Models;
using RmlBlogMvc.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RmlBlogMvc.Service
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            applicationDbContext = dbContext;
        }

        public async Task<User> GetUserById(string userid)
        {
            var user = await applicationDbContext.Users
                .FirstOrDefaultAsync(x => x.Id == userid);
            return user;
        }

        public async Task Update(User user)
        {
            applicationDbContext.Update(user);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
