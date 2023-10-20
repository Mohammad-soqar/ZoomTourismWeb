using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models
{
    public class ATask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        public string AssignedUserId { get; set; }
        public ApplicationUser AssignedUser { get; set; }

        [ValidateNever]
        public int? LeadId { get; set; }
        [ValidateNever]
        public Lead Lead { get; set; }
    }
}
