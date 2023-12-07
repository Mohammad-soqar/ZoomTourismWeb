using Amazon.S3.Model;
using Amazon.S3;
using ZoomTourism.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models;
using ZoomTourism.Models.ViewModels;
using Amazon;

namespace ZoomTourismWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_CodyleSupport)]
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
            var leadList = _unitOfWork.Lead.GetAll();
            var CarList = _unitOfWork.Car.GetAll();
            var TaskList = _unitOfWork.ATask.GetAll();
            var ReviewList = _unitOfWork.Review.GetAll();
            var salesData = _unitOfWork.Sale.GetAll()
                .Where(s => s.SaleDate >= twoMonthsAgoStartDate && s.SaleDate <= currentMonthStartDate)
                .ToList();
            var averageRating = ReviewList.Any() ? (int)(ReviewList.Average(review => review.Rating) * 20) : 0;

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
                CarCount = CarList.Count(),
                ReviewCount = ReviewList.Count(),
                TaskCount = TaskList.Count(),
                ReviewPercentage = averageRating,
                SalesChartVM = salesByMonth,
                Lead = leadList,
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

        public IActionResult SupportReport(int? Id)
        {
            Report report = new Report();

            if (Id == null || Id == 0)
            {
                return View(report);
            }
            else
            {
                report = _unitOfWork.Report.GetFirstOrDefault(u => u.Id == Id);
                return View(report);
            }

           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupportReport(Report obj, IFormFile file)
        {

            IdentityUser user = _userManager.GetUserAsync(User).Result;

            IdentityUser CurrentUser = _userManager.Users.FirstOrDefault(s => s.Id == user.Id);
            obj.UserId = CurrentUser.Id;

            if (ModelState.IsValid)
            {
                string wwwRootPath = _HostEnvironment.WebRootPath;
                if (file != null && file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        var extension = Path.GetExtension(file.FileName);
                        var fileName = Guid.NewGuid().ToString() + extension;

                        // Use the AmazonS3Client to upload the file to S3
                        var s3Client = new AmazonS3Client("AKIA2VXAQTX6WKS7IIHB", "CGvzFn0noWJSAKMKbT7It2eNSxzuJk9ZwVS6N/Bo", RegionEndpoint.EUCentral1);
                        var putRequest = new PutObjectRequest
                        {
                            BucketName = "zoomtourismassets",
                            Key = "Images/Report/" + fileName,
                            InputStream = memoryStream,
                            ContentType = file.ContentType,
                            CannedACL = S3CannedACL.PublicRead // Optional: Set appropriate ACL for your use case
                        };

                        await s3Client.PutObjectAsync(putRequest);

                        obj.ImageUrl = "https://zoomtourismassets.s3.eu-central-1.amazonaws.com/Images/Report/" + fileName;
                    }
                }
                if (obj.Id == 0)
                {
                    _unitOfWork.Report.Add(obj);
                }
                else
                {
                    _unitOfWork.Report.Update(obj);
                }


                _unitOfWork.Save();
                return RedirectToAction("AdminDashboard");
            }

            return View(obj);

        }

        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_CodyleSupport)]

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
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_CodyleSupport)]

        public IActionResult WebsiteContentManagement()
        {
            return View();
        }
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_CodyleSupport)]

        public IActionResult TripsDashboard()
        {
            IEnumerable<Trip> TripList = _unitOfWork.Trip.GetAll();
            return View(TripList);
        }
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_CodyleSupport)]

        public IActionResult BlogsDashboard()
        {
            IEnumerable<Blog> BlogList = _unitOfWork.Blog.GetAll();
            return View(BlogList);
        }

        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_CodyleSupport)]
        public IActionResult CarsDashboard()
        {
            IEnumerable<Car> CarList = _unitOfWork.Car.GetAll();
            return View(CarList);
        }
    }
}
