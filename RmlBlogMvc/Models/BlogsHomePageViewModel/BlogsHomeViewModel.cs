using PagedList.Core;
using RmlBlogMvc.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RmlBlogMvc.Models.BlogsHomePageViewModel
{
    public class BlogsHomeViewModel
    {
        public string SearchRequest { get; set; }
        public int CurrentPage { get; set; }
        public IPagedList<Blog> BlogsPagedList { get; set; }
    }
}
