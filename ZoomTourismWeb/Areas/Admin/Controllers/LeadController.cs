
using CodyleOffical.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models;
using ZoomTourism.Models.ViewModels;

namespace ZoomTourism.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class LeadController : Controller
    {
     
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _HostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public LeadController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment,
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _HostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? Id)
        {
            LeadVM leadVm = new() {
                Lead = new()
            };

           
            // Load users for CallCenter role
            var callCenterUsers = _userManager.GetUsersInRoleAsync("CallCenter").Result;
            leadVm.CallCenterList = new SelectList(callCenterUsers, "Id", "UserName");

            // Load users for BookingDep role
            var bookingDepUsers = _userManager.GetUsersInRoleAsync("Booking").Result;
            leadVm.BookingList = new SelectList(bookingDepUsers, "Id", "UserName");

            // Load users for Driver role
            var driverUsers = _userManager.GetUsersInRoleAsync("Driver").Result;
            leadVm.DriverList = new SelectList(driverUsers, "Id", "UserName");

            if (Id == null || Id == 0)
            {
                return View(leadVm);
            }
            else
            {
                var lead = _unitOfWork.Lead.GetFirstOrDefault(u => u.Id == Id);
                leadVm.Lead = lead;
                

                return View(leadVm);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(LeadVM leadVm)
        {
            if (ModelState.IsValid)
            {
                Lead obj = leadVm.Lead;
                if (!string.IsNullOrEmpty(obj.CallCenterUserId))
                {
                    // Assign Call Center user based on obj.CallCenterUserId
                    var callCenterUser = _userManager.FindByIdAsync(obj.CallCenterUserId).Result;
                    obj.CallCenter = (ApplicationUser)callCenterUser;
                }

                if (!string.IsNullOrEmpty(obj.BookingDepUserId))
                {
                    // Assign Booking Department user based on obj.BookingDepUserId
                    var bookingDepUser = _userManager.FindByIdAsync(obj.BookingDepUserId).Result;
                    obj.BookingDep = (ApplicationUser)bookingDepUser;
                }

                if (!string.IsNullOrEmpty(obj.DriverUserId))
                {
                    // Assign Driver user based on obj.DriverUserId
                    var driverUser = _userManager.FindByIdAsync(obj.DriverUserId).Result;
                    obj.Driver = (ApplicationUser)driverUser;
                }


                if (obj.Id == 0)
                {
                    _unitOfWork.Lead.Add(obj);
                }
                else
                {
                    _unitOfWork.Lead.Update(obj);
                }

                if (obj.IsPaid)
                {
                    var existingSale = _unitOfWork.Sale.GetFirstOrDefault(s => s.LeadId == obj.Id);

                    if (existingSale != null)
                    {
                        existingSale.SaleAmount = obj.SaleAmount;
                        _unitOfWork.Sale.Update(existingSale);
                    }
                    else
                    {
                        var sale = new Sale
                        {
                            ProductType = obj.LeadType,
                            SaleAmount = obj.SaleAmount,
                            SaleDate = DateTime.Now,
                            LeadId = obj.Id
                        };
                        _unitOfWork.Sale.Add(sale);
                    }
                }

                if(obj.Status.ToString().ToLower() == "finished")
                {
                    var existingReview = _unitOfWork.ReviewLink.GetFirstOrDefault(s => s.LeadId == obj.Id);

                    if (existingReview == null)
                    {
                        string reviewToken = GenerateReviewToken();
                        string phoneNum = obj.Phone;
                        var reviewLink = new ReviewLink
                        {
                            Token = reviewToken,
                            LeadId = obj.Id
                        };
                        _unitOfWork.ReviewLink.Add(reviewLink);
                        _unitOfWork.Save();

                        string reviewUrl = $"https://localhost:44346/Customer/Home/ReviewUpsert?token={reviewToken}";

                        return RedirectToAction("SendSms", "Sms", new { reviewLink = reviewUrl , phoneNumber = phoneNum });
                    }
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(leadVm);
        }

        private string GenerateReviewToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }




        private void DisplayReviewLink(string reviewUrl)
        {
            // For testing purposes, you can display the review link in the console or on a webpage
            Console.WriteLine("Review Link: " + reviewUrl);

            // If you are working in a web application, you can send the link to the view for display
            // ViewBag.ReviewLink = reviewUrl;
            // or
            // ViewData["ReviewLink"] = reviewUrl;
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

            return View(salesByMonth);
        }

        


      



        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            
            return View();

        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var leadList = _unitOfWork.Lead.GetAll();
            return Json(new { data = leadList });
        }

        [HttpDelete]

        public IActionResult DeletePost(int? Id)
        {
            var obj = _unitOfWork.Lead.GetFirstOrDefault(u => u.Id == Id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            

            _unitOfWork.Lead.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Lead deleted successfuly" });
            return RedirectToAction("Index");

            return View(Id);

        }


        #endregion


    }
}
