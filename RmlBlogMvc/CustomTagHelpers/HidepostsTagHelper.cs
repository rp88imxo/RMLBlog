using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RmlBlogMvc.CustomTagHelpers
{
    public class HidepostsTagHelper : TagHelper
    {
        public int Totall { get; set; }
        public int MinCount { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            if (Totall <= MinCount)
            {
                output.SuppressOutput();
            }

        }
    }
}
