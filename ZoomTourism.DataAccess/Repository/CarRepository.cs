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
    public class CarRepository : Repository<Car>, ICarRepository
    {
        private readonly ApplicationDbContext _db;
       
        public CarRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
            
        }

      
        public void Update(Car obj)
        {
            var objFromDb = _db.Cars.FirstOrDefault(u=>u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.CarBrand = obj.CarBrand;
                objFromDb.ModelName = obj.ModelName;
                objFromDb.CarCardImage = obj.CarCardImage;
                objFromDb.ModelYear = obj.ModelYear;
                objFromDb.TypeOfTransmission = obj.TypeOfTransmission;
                objFromDb.NumOfSeats = obj.NumOfSeats;
                objFromDb.TypeOfFuel = obj.TypeOfFuel;
                objFromDb.SecurityDeposit = obj.SecurityDeposit;
                objFromDb.ExcessClaim = obj.ExcessClaim;
                objFromDb.MilageCharge = obj.MilageCharge;
                objFromDb.MinimumEligibleAge = obj.MinimumEligibleAge;
                objFromDb.InsuranceIncluded = obj.InsuranceIncluded;
                objFromDb.DailyCharge = obj.DailyCharge;
                objFromDb.WeeklyCharge = obj.WeeklyCharge;
                objFromDb.MonthlyCharge = obj.MonthlyCharge;

              

            }
        }
    }
}
