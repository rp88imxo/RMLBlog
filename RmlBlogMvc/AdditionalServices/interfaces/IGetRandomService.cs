using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RmlBlogMvc.AdditionalServices.interfaces
{
    public interface IGetRandomService
    {
        Random rand { get; }
        int GetRandomNumber(int min, int max);
       
    }
}
