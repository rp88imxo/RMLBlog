using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RmlBlogMvc.Data.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }

        public string UserAboutInfoHeader { get; set; }
        public string UserAboutInfoContent { get; set; }
        public bool UserAboutInfoAllowedToOthers { get; set; }
    }
}
