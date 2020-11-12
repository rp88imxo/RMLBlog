using RmlBlogMvc.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RmlBlogMvc.Models.AdminDashboardViewModel
{
    public class DashboardViewModel
    {
        public IEnumerable<Blog> Blogs { get; set; }
    }
}
