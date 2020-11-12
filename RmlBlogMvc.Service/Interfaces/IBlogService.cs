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
        public IQueryable<Blog> GetBlogs(User user);
        Task<Blog> Add(Blog blog);
    }
}
