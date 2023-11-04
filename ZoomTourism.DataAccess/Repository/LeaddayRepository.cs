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
    public class LeaddayRepository : Repository<LeadDay>, ILeaddayRepository
    {
        private readonly ApplicationDbContext _db;
       
        public LeaddayRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
            
        }


        public void Update(LeadDay obj)
        {
            var objFromDb = _db.LeadDays.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Destination = obj.Destination;
                objFromDb.numOfDays = obj.numOfDays;
               

            }
        }

    }
}
