using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }
        [ValidateNever]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; } 

        [ValidateNever]
        public int numOfReviews { get; set; } = 0;
    }

}
