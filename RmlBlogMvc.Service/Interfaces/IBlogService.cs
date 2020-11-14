using RmlBlogMvc.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RmlBlogMvc.Service.Interfaces
{
   public interface IBlogService
    {
        Task<Blog> Update(Blog blog);
        public IQueryable<Blog> GetBlogs(User user);
        Task<Blog> Add(Blog blog);
        Task<Blog> GetBlogById(int BlogId);
        Task<int> Delete(Blog blog);
        IQueryable<Blog> GetBlogsBySearchRequest(string searchRequest);
    }
}
