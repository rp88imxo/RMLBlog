using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RmlBlogMvc.Models.UserViewModel
{
    public class UserAboutInfoViewModel
    {
        [Display(Name ="Your page image")]
        public IFormFile AboutImage { get; set; }
        [Required]
        [MinLength(length:3, ErrorMessage ="Title is too short, min chars is {1}")]
        [Display(Name ="Title")]
        public string AboutInfoHeader { get; set; }
        [Required]
        [Display(Name = "Info about you")]
        public string AboutInfoContent { get; set; }
        [Required]
        public bool AllowedToOthers { get; set; }
    }
}
