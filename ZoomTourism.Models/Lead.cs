using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models
{
    public class Lead
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfDays { get; set; }
        public List<string> Destinations { get; set; }
        public int NumberOfPeople { get; set; }
        public bool IsPaid { get; set; }
        [ValidateNever]
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }
        public LeadStatus Status { get; set; }
        public string Notes { get; set; }
        [ValidateNever]
        public int? SelectedCarId { get; set; }
        [ValidateNever]
        public Car? SelectedCar { get; set; }
        [ValidateNever]
        public DateTime? CarPickupDate { get; set; }
        [ValidateNever]
        public DateTime? CarReturnDate { get; set; }
        [ValidateNever]
        public DateTime? HotelCheckInDate { get; set; }
        [ValidateNever]
        public DateTime? HotelCheckOutDate { get; set; }
        [ValidateNever]
        public string? HotelName { get; set; }
        [ValidateNever]
        public decimal? HotelTotalCost { get; set; }

       
    }

    public enum LeadStatus
    {
        New,
        Contacted,
        Converted,
        Lost
    }
}
