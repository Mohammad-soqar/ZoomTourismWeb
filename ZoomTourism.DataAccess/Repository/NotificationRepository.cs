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
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        private readonly ApplicationDbContext _db;
       
        public NotificationRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
            
        }


        public void Update(Notification obj)
        {
            var objFromDb = _db.Notifications.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.IsRead = obj.IsRead;



            }
        }

    }
}
