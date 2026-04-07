using Application.DTOs.Administrator.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboarDto> GetDashboardDataAsync();
    }
}
