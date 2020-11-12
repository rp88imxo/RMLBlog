using System;
using System.Collections.Generic;
using System.Text;

namespace RmlBlogMvc.Data.Models
{
    public class Post
    {
        public int Id { get; set; }
        public Blog Blog { get; set; }
        public DateTime CreationTime { get; set; }
        public User PostCreator { get; set; }
        public string Content { get; set; }
        public Post RelatedPost { get; set; }
    }
}
