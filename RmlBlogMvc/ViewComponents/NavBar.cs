using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RmlBlogMvc.ViewComponents
{
    public class NavBar : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
