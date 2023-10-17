using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models.ViewModels
{
    public class SalesChartVM
    {
        public string Month { get; set; }
        public decimal TotalSales { get; set; }
        public int SalesCount { get; set; }
    }
}
