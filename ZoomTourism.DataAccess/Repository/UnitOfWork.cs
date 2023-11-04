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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Blog = new BlogRepository(_db);
            Trip = new TripRepository(_db);
            Car = new CarRepository(_db);
            CarImage = new CarimagesRepository(_db);
            Lead = new LeadRepository(_db);
            Sale = new SaleRepository(_db);
            Review = new ReviewRepository(_db);
            ReviewLink = new ReviewlinkRepository(_db);
            ATask = new TaskRepository(_db);
            LeadDay = new LeaddayRepository(_db);
            Report = new ReportRepository(_db);
            ApplicationUser = new ApplicationuserRepository(_db);


        }

        public ICategoryRepository Category {get; private set;}
        public IBlogRepository Blog { get; private set; }
        public ITripRepository Trip { get; private set; }
        public ICarRepository Car { get; private set; }
        public ICarimagesRepository CarImage { get; private set; }
        public ILeadRepository Lead { get; private set; }
        public ISaleRepository Sale { get; private set; }
        public IReviewRepository Review { get; private set; }
        public IReviewlinkRepository ReviewLink { get; private set; }
        public ITaskRepository ATask { get; private set; }
        public ILeaddayRepository LeadDay { get; private set; }
        public IReportRepository Report { get; private set; }
        public IApplicationuserRepository ApplicationUser { get; private set; }


        public void Save()
        {
           _db.SaveChanges();
        }
    }
}
