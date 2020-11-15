using Microsoft.AspNetCore.Razor.TagHelpers;
using PagedList.Core;
using RmlBlogMvc.CONST_PATHS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RmlBlogMvc.CustomTagHelpers
{
    
    public class PageListHiderTagHelper : TagHelper
    {
        public int Totall { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            if (Totall <= ConstInfo.BLOGS_ON_PAGE_COUNT)
            {
                output.SuppressOutput();
            }
            
        }
    }
}
