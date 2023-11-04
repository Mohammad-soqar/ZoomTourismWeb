using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models.ViewModels
{
    public class LeadVM
    {
        public Lead Lead { get; set; }
       

        [ValidateNever]
        public List<LeadDay> LeadDay { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CarList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CallCenterList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> BookingList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> DriverList { get; set; }
    }
}
