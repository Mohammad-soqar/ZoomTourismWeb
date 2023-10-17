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
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        private readonly ApplicationDbContext _db;
       
        public SaleRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
            
        }


        public void Update(Sale obj)
        {
            var objFromDb = _db.Sales.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.SaleAmount = obj.SaleAmount;
                
                
              
            }
        }

    }
}
