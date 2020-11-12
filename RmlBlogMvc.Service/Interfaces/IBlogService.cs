using RmlBlogMvc.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RmlBlogMvc.Service.Interfaces
{
   public interface IBlogService
    {
        Task<Blog> Add(Blog blog);
    }
}
