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
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _db;
       
        public ReviewRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
            
        }


        public void Update(Review obj)
        {
            var objFromDb = _db.Reviews.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Comment = obj.Comment;
                objFromDb.Rating = obj.Rating;



            }
        }

    }
}
