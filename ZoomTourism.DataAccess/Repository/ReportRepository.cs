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
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        private readonly ApplicationDbContext _db;
       
        public ReportRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
            
        }

      
        public void Update(Report obj)
        {
            var objFromDb = _db.Reports.FirstOrDefault(u=>u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Description = obj.Description;
             

            }
        }
    }
}
