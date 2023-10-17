using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string ProductType { get; set; } // e.g., "Trip", "Car"
        public decimal? SaleAmount { get; set; }
        public DateTime SaleDate { get; set; }
        public int LeadId { get; set; }
        public Lead Lead { get; set; }
    }
}
