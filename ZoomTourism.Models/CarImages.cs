using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models
{
    public class CarImages
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        [ValidateNever]
        public Car Car { get; set; }
        public string ImageUrl { get; set; }

    }
}
