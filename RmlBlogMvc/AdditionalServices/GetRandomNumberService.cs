using RmlBlogMvc.AdditionalServices.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RmlBlogMvc.AdditionalServices
{
    public class GetRandomNumberService : IGetRandomService
    {
        public Random rand { get; }

        public GetRandomNumberService()
        {
            rand = new Random();
        }

        public int GetRandomNumber(int min, int max)
        {
            return rand.Next(min, max);
        }
    }
}
