using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoomTourism.Models;

namespace ZoomTourism.DataAccess.Repository.IRepository
{
    public interface IReportRepository : IRepository<Report>
    {
        void Update(Report obj);
     
    }
}
