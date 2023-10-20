using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models
{
    public class ReviewLink
    {
        public int Id { get; set; }
        public string Token { get; set; } // The unique token associated with the review link
        public int LeadId { get; set; } // The ID of the lead associated with the review link
        public Lead Lead { get; set; }
        public bool IsUsed { get; set; } // A flag to mark if the link has been used

    }
}
