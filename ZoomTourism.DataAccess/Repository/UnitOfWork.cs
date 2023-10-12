using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoomTourism.DataAccess.Data;
using ZoomTourism.DataAccess.Repository.IRepository;

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


        }

        public ICategoryRepository Category {get; private set;}
        public IBlogRepository Blog { get; private set; }
        public ITripRepository Trip { get; private set; }
        public ICarRepository Car { get; private set; }
        public ICarimagesRepository CarImage { get; private set; }
        public ILeadRepository Lead { get; private set; }


        public void Save()
        {
           _db.SaveChanges();
        }
    }
}
