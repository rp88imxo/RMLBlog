using Microsoft.EntityFrameworkCore;
using RmlBlogMvc.Data;
using RmlBlogMvc.Data.Models;
using RmlBlogMvc.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RmlBlogMvc.Service
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public BlogService(ApplicationDbContext dbContext)
        {
            applicationDbContext = dbContext;
        }
        public async Task<Blog> Add(Blog blog)
        {
            applicationDbContext.Add(blog);
            await applicationDbContext.SaveChangesAsync();
            return blog;
        }

        public IQueryable<Blog> GetBlogs(User user)
        {
            var blogs = applicationDbContext.Blogs
                .Include(x => x.BlogCreator)
                .Include(x => x.ApprovedByUser)
                .Include(x => x.Posts)
                .Where(x => x.BlogCreator.Id == user.Id);
            return blogs;
        }

        public async Task<Blog> GetBlogById(int BlogId)
        {
            return await applicationDbContext.Blogs
                .Include(x => x.BlogCreator)
                .Include(x => x.ApprovedByUser)
                .Include(x => x.Posts)
                .FirstOrDefaultAsync(x => x.Id == BlogId);
        }

        public async Task<Blog> Update(Blog blog)
        {
            applicationDbContext.Update(blog);
            await applicationDbContext.SaveChangesAsync();
            return blog;
        }
        public async Task<int> Delete(Blog blog)
        {
            applicationDbContext.Remove(blog);
            return await applicationDbContext.SaveChangesAsync();
        }
        public IQueryable<Blog> GetBlogsBySearchRequest(string searchRequest)
        {
            return applicationDbContext.Blogs
                .OrderByDescending(x => x.EditedTime)
                .Include(x => x.BlogCreator)
                .Include(x => x.Posts)
                .Where(x => x.Title.Contains(searchRequest) || x.Content.Contains(searchRequest))
                .AsQueryable();
                
        }
    }
}
