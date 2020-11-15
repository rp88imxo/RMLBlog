using RmlBlogMvc.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RmlBlogMvc.Service.Interfaces
{
    public interface IPostService
    {
        Task<Post> GetPostById(int id);
        Task Delete(Post post);
        Task Add(Post post);
        Task Update(Post post);
    }
}
