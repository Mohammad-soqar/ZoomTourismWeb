using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models.ViewModels
{
    public class TaskVM
    {
        public ATask Task { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> UserList { get; set; }
    }
}
