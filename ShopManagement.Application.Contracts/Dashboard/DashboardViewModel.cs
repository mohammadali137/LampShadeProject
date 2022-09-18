using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Application.Contracts.Dashboard
{
    public class DashboardViewModel
    {
        public double OrderCount { get; set; }
        public int ProductCount { get; set; }
        public int UsersCount { get; set; }
        public int VisitSiteCount { get; set; }
    }
}
