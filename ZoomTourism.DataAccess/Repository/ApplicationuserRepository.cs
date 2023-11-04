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
    public class ApplicationuserRepository : Repository<ApplicationUser>, IApplicationuserRepository
    {
        private readonly ApplicationDbContext _db;
       
        public ApplicationuserRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
            
        }

      
        public void Update(ApplicationUser obj)
        {
          
        }
    }
}
