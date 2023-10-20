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
    public class ReviewlinkRepository : Repository<ReviewLink>, IReviewlinkRepository
    {
        private readonly ApplicationDbContext _db;
       
        public ReviewlinkRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
            
        }


        public void Update(ReviewLink obj)
        {
            var objFromDb = _db.ReviewLinks.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Token = obj.Token;
                objFromDb.Lead = obj.Lead;
                objFromDb.IsUsed = obj.IsUsed;



            }
        }

    }
}
