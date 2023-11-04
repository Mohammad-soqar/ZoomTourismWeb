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
    public class TripRepository : Repository<Trip>, ITripRepository
    {
        private readonly ApplicationDbContext _db;
       
        public TripRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
            
        }

      
        public void Update(Trip obj)
        {
            var objFromDb = _db.Trips.FirstOrDefault(u=>u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Content = obj.Content;
                objFromDb.CreatedDate = obj.CreatedDate;
                objFromDb.Price = obj.Price;

                if (objFromDb.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }

            }
        }
    }
}
