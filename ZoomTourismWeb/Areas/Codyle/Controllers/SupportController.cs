using CodyleOffical.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models;

namespace ZoomTourismWeb.Areas.Codyle.Controllers
{
    [Area("Codyle")]
    [Authorize(Roles = SD.Role_CodyleSupport)]
    public class SupportController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _HostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public SupportController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment,
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _HostEnvironment = hostEnvironment;
            _userManager = userManager;
        }
        public IActionResult Reports()
        {
            IEnumerable<Report> ReportsList = _unitOfWork.Report.GetAll();
            return View(ReportsList);
        }
    }
}
