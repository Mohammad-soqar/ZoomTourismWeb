using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string AssignedUserId { get; set; }
        [ValidateNever]
        public ApplicationUser AssignedUser { get; set; }
        public bool IsRead { get; set; }
        [ValidateNever]
        public string leadLink { get; set; }
        [ValidateNever]
        public string taskLink { get; set; }


    }
}
