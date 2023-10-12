using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string CarBrand { get; set; }
        public string ModelName { get; set; }
        public string ModelYear { get; set; }
        public string TypeOfTransmission { get; set; }
        public int NumOfSeats { get; set; }
        public string TypeOfFuel { get; set; }

        public string SecurityDeposit { get; set; }
        public string ExcessClaim { get; set; }
        public int MilageCharge { get; set; }
        public int MinimumEligibleAge { get; set; } = 21;
        public bool InsuranceIncluded { get; set; }

        public int DailyCharge { get; set; }
        public int WeeklyCharge { get; set; }
        public int MonthlyCharge { get; set; }
        [ValidateNever]
        public string? ExistingImageUrls { get; set; }

    }
}
