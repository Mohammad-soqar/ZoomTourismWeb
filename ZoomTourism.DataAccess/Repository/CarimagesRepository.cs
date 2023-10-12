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
    public class CarimagesRepository : Repository<CarImages>, ICarimagesRepository
    {
        private readonly ApplicationDbContext _db;
       
        public CarimagesRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
            
        }

      
        public void Update(CarImages obj)
        {
            var objFromDb = _db.CarImages.FirstOrDefault(u=>u.Id == obj.Id);
            if (objFromDb != null)
            {
                if (objFromDb.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }

            }
        }
    }
}
