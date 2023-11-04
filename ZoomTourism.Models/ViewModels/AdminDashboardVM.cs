using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models.ViewModels
{
    public class AdminDashboardVM
    {
        public List<SalesChartVM> SalesChartVM { get; set; }
        public IEnumerable<Lead> Lead { get; set; }
        public int CarCount { get; set; }
        public int TaskCount { get; set; }
        public int ReviewCount { get; set; }
        public int ReviewPercentage { get; set; }
    }
}
