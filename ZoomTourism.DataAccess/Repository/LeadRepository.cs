using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoomTourism.DataAccess.Data;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models;

namespace ZoomTourism.DataAccess.Repository
{
    public class LeadRepository : Repository<Lead>, ILeadRepository
    {
        private readonly ApplicationDbContext _db;
       
        public LeadRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
            
        }


        public void Update(Lead obj)
        {
            var objFromDb = _db.Leads.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.NumberOfDays = obj.NumberOfDays;
                objFromDb.Destinations = obj.Destinations;
                objFromDb.NumberOfPeople = obj.NumberOfPeople;
                objFromDb.IsPaid = obj.IsPaid;
                objFromDb.Phone = obj.Phone;
                objFromDb.CreatedDate = obj.CreatedDate;
                objFromDb.Status = obj.Status;
                objFromDb.Notes = obj.Notes;
                objFromDb.CallCenterUserId = obj.CallCenterUserId;
                objFromDb.CallCenter = obj.CallCenter;
                objFromDb.BookingDepUserId = obj.BookingDepUserId;
                objFromDb.BookingDep = obj.BookingDep;
                objFromDb.DriverUserId = obj.DriverUserId;
                objFromDb.Driver = obj.Driver;

                // Check and update properties that are not null
                if (obj.SelectedCarId.HasValue)
                {
                    objFromDb.SelectedCarId = obj.SelectedCarId;
                }

                if (obj.SelectedCar != null)
                {
                    objFromDb.SelectedCar = obj.SelectedCar;
                }

                if (obj.CarPickupDate != null)
                {
                    objFromDb.CarPickupDate = obj.CarPickupDate;
                }

                if (obj.CarReturnDate != null)
                {
                    objFromDb.CarReturnDate = obj.CarReturnDate;
                }

                if (obj.HotelCheckInDate != null)
                {
                    objFromDb.HotelCheckInDate = obj.HotelCheckInDate;
                }

                if (obj.HotelCheckOutDate != null)
                {
                    objFromDb.HotelCheckOutDate = obj.HotelCheckOutDate;
                }

                if (obj.HotelName != null)
                {
                    objFromDb.HotelName = obj.HotelName;
                }

                if (obj.HotelTotalCost.HasValue)
                {
                    objFromDb.HotelTotalCost = obj.HotelTotalCost;
                }
            }
        }

    }
}
