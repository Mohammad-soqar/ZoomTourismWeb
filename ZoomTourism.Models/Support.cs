using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        [ValidateNever]
        public ApplicationUser? User { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        [ValidateNever]
        public string PhoneNumber { get; set; }

    }
}
