using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        [ValidateNever]

        public ApplicationUser ApplicationUser { get; set; }

        [ValidateNever]
        public int? TripId { get; set; }
        [ValidateNever]
        public Trip? Trip { get; set; }
        public int LeadId { get; set; }
        [ValidateNever]

        public Lead Lead { get; set; }

    }
}
