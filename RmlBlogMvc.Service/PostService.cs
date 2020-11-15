using Microsoft.EntityFrameworkCore;
using RmlBlogMvc.Data;
using RmlBlogMvc.Data.Models;
using RmlBlogMvc.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RmlBlogMvc.Service
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext applicationDbContext;
        public PostService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Post> GetPostById(int id)
        {
            return await applicationDbContext.Posts
                .Include(x => x.PostCreator)
                .Include(x => x.RelatedPost)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Add(Post post)
        {
            applicationDbContext.Add(post);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(Post post)
        {
            applicationDbContext.Remove(post);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task Update(Post post)
        {
            applicationDbContext.Update(post);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
