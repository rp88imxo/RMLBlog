using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RmlBlogMvc.AdditionalServices.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RmlBlogMvc.ViewComponents
{
    public class Header : ViewComponent
    {
        private readonly IGetRandomService getRandomNumService;
        private readonly IConfiguration config;

        public Header(IGetRandomService randomService, IConfiguration configuration)
        {
            getRandomNumService = randomService;
            config = configuration;
        }
        public IViewComponentResult Invoke()
        {
            string path = config["bgImagesHomePath"] + getRandomNumService.GetRandomNumber(0,5).ToString() + ".jpg";

            return View("Default", path);
        }
    }
}
