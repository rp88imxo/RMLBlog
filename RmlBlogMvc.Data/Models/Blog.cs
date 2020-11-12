using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RmlBlogMvc.Data.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public User BlogCreator { get; set; }
        
        [Required(AllowEmptyStrings =false, ErrorMessage ="Title is not valid!")]
        [MinLength(length:3, ErrorMessage = "Min title length is {1}")]
        public string Title { get; set; }
        
        [Required(AllowEmptyStrings =false, ErrorMessage ="Content is not valid!")]
        [MinLength(length:8, ErrorMessage ="Provide more info about your blog, min length is {1}")]
        public string Content { get; set; }
        
        public DateTime CreationTime { get; set; }
        public bool isAllowedToPost { get; set; }
        public User ApprovedByUser { get; set; } // Approver of the blog to be post
        public bool Published { get; set; }

        public virtual IEnumerable<Post> Posts { get; set; }
    }
}
