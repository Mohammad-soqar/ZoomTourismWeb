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
    public class TaskRepository : Repository<ATask>, ITaskRepository
    {
        private readonly ApplicationDbContext _db;
       
        public TaskRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
            
        }


        public void Update(ATask obj)
        {
            var objFromDb = _db.Tasks.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.DueDate = obj.DueDate;
                objFromDb.LeadId = obj.LeadId;
                objFromDb.Lead = obj.Lead;
                objFromDb.AssignedUserId = obj.AssignedUserId;
                objFromDb.AssignedUser = obj.AssignedUser;
                objFromDb.TaskPriority = obj.TaskPriority;
                objFromDb.TaskStatus = obj.TaskStatus;

            }
        }

    }
}
