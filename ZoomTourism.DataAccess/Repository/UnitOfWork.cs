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
          

        }

        public ICategoryRepository Category {get; private set;}
        public IBlogRepository Blog { get; private set; }
      

        public void Save()
        {
           _db.SaveChanges();
        }
    }
}
