using CodyleOffical.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models.ViewModels;

namespace ZoomTourismWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _HostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment,
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _HostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        public IActionResult AdminDashboard()
        {
            var currentDate = DateTime.Now;
            var currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1);
            var lastMonthStartDate = currentMonthStartDate.AddMonths(-1);
            var twoMonthsAgoStartDate = currentMonthStartDate.AddMonths(-3);

            // Retrieve sales data for the past two months and the current month
            var salesData = _unitOfWork.Sale.GetAll()
                .Where(s => s.SaleDate >= twoMonthsAgoStartDate && s.SaleDate <= currentMonthStartDate)
                .ToList();

            // Calculate the total sales amount and sales count for each month
            var salesByMonth = salesData
                .GroupBy(s => s.SaleDate.ToString("yyyy-MM"))
                .Select(g => new SalesChartVM
                {
                    Month = g.Key,
                    TotalSales = g.Sum(s => s.SaleAmount ?? 0),
                    SalesCount = g.Count()
                })
                .ToList();

            

            var dashboardData = new AdminDashboardVM
            {
                SalesChartVM = salesByMonth,
                // Set other properties as needed
            };

            return View(dashboardData);
        }
        public IActionResult CallCenterDashboard()
        {
            return View();
        }

        public IActionResult BookingDepartmentDashboard()
        {
            return View();
        }

        public IActionResult SalesChart()
        {
            var currentDate = DateTime.Now;
            var currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1);
            var lastMonthStartDate = currentMonthStartDate.AddMonths(-1);
            var twoMonthsAgoStartDate = currentMonthStartDate.AddMonths(-3);

            // Retrieve sales data for the past two months and the current month
            var salesData = _unitOfWork.Sale.GetAll()
                .Where(s => s.SaleDate >= twoMonthsAgoStartDate && s.SaleDate <= currentMonthStartDate)
                .ToList();

            // Calculate the total sales amount and sales count for each month
            var salesByMonth = salesData
                .GroupBy(s => s.SaleDate.ToString("yyyy-MM"))
                .Select(g => new SalesChartVM
                {
                    Month = g.Key,
                    TotalSales = g.Sum(s => s.SaleAmount ?? 0),
                    SalesCount = g.Count()
                })
                .ToList();

            return PartialView("_SalesChartPartial", salesByMonth);
        }
    }
}
