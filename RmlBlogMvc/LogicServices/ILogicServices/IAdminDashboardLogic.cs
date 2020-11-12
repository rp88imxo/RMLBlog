using RmlBlogMvc.Models.AdminDashboardViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RmlBlogMvc.LogicServices.ILogicServices
{
    public interface IAdminDashboardLogic
    {
        Task<DashboardViewModel> GetDashboard(ClaimsPrincipal claimsPrincipal);
    }
}
