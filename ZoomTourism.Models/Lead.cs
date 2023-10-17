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
        public int Id { get; set; }//no need
        public string LeadType { get; set; }

        //General
        public string Name { get; set; }
        [ValidateNever]
        public string? Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; } //no need
        public LeadStatus Status { get; set; } //no need
        public string Notes { get; set; }

        [ValidateNever]
        public decimal? SaleAmount { get; set; } //only when IsPaid is checked
        public bool IsPaid { get; set; }


        public string CallCenterUserId { get; set; }
        [ValidateNever]
        public ApplicationUser CallCenter { get; set; }
        public string BookingDepUserId { get; set; }
        [ValidateNever]
        public ApplicationUser BookingDep { get; set; }
        public string DriverUserId { get; set; }
        [ValidateNever]
        public ApplicationUser Driver { get; set; }



        //Private Trip
        [ValidateNever]
        public string? Destinations { get; set; }
        [ValidateNever]
        public int? NumberOfPeople { get; set; }
        [ValidateNever]
        public DateTime? HotelCheckInDate { get; set; }
        [ValidateNever]
        public DateTime? HotelCheckOutDate { get; set; }
        [ValidateNever]
        public string? HotelName { get; set; }
        [ValidateNever]
        public decimal? HotelTotalCost { get; set; }
       



        //Private Trip + Car Rental
        [ValidateNever]
        public int? SelectedCarId { get; set; }
        [ValidateNever]
        public Car? SelectedCar { get; set; }
        [ValidateNever]
        public DateTime? CarPickupDate { get; set; }
        [ValidateNever]
        public DateTime? CarReturnDate { get; set; }
        [ValidateNever]
        public int? NumberOfDays { get; set; }
        //Public Trip
        //nothing yet

        //Car Rental
        //nothing yet

    }

    public enum LeadStatus
    {
        New,
        Contacted,
        Converted,
        Lost
    }
}
