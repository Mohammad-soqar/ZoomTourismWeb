using ZoomTourism.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }

        IBlogRepository Blog { get; }
        ITripRepository Trip { get; }
        ICarRepository Car { get; }
        ICarimagesRepository CarImage { get; }
        ILeadRepository Lead { get; }
        ISaleRepository Sale { get; }
        IReviewRepository Review { get; }


        void Save();
    }
}
