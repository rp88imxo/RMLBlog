using Microsoft.AspNetCore.Http;
using RmlBlogMvc.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RmlBlogMvc.Models.BlogViewModel
{
    public class DeleteBlogViewModel
    {
        [DisplayName("Blog Image")]
        public IFormFile BlogHeaderImage { get; set; }
        [Required]
        public Blog Blog { get; set; }
    }
}
