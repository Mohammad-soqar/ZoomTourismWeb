
using CodyleOffical.Utility;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using Twilio.TwiML.Voice;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models;
using ZoomTourism.Models.ViewModels;
using ZoomTourismWeb;

namespace ZoomTourism.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_CallCenter + "," + SD.Role_CodyleSupport)]
    public class LeadController : Controller
    {
     
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _HostEnvironment;
        private readonly UserManager<IdentityUser> _userManager; 
        private readonly SmsReminderService _smsReminderService;

        public LeadController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment,
            UserManager<IdentityUser> userManager, SmsReminderService smsReminderService)
        {
            _unitOfWork = unitOfWork;
            _HostEnvironment = hostEnvironment;
            _userManager = userManager;
            _smsReminderService = smsReminderService;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Leads()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool isAdmin = User.IsInRole(SD.Role_Admin);

            IEnumerable <Lead> leads;

            if (isAdmin || User.IsInRole(SD.Role_CodyleSupport))
            {
                leads = _unitOfWork.Lead.GetAll();
                return View(leads);
            }
            else if (User.IsInRole(SD.Role_CallCenter))
            {
                leads = _unitOfWork.Lead.GetAll()
                    .Where(lead => lead.CallCenterUserId == userId);
                return View(leads);
            }
            else if (User.IsInRole(SD.Role_Booking))
            {
                leads = _unitOfWork.Lead.GetAll()
                    .Where(lead => lead.BookingDepUserId == userId);
                return View(leads);
            }
            else if (User.IsInRole(SD.Role_Driver))
            {
                leads = _unitOfWork.Lead.GetAll()
                    .Where(lead => lead.BookingDepUserId == userId);
                return View(leads);
            }

            return View();
        }

        public IActionResult Upsert(int? Id)
        {
            LeadVM leadVm = new() {
                Lead = new Lead(),
                CarList = _unitOfWork.Car.GetAll().Select(i => new SelectListItem
                {
                    Text = i.ModelName,
                    Value = i.Id.ToString()
                })
            };

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

                // Retrieve and populate LeadDays for the existing lead
                var leadDays = _unitOfWork.LeadDay.GetAll(ld => ld.LeadId == Id).ToList();
                leadVm.LeadDay = leadDays;

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
                    _unitOfWork.Save(); // Save to get the generated Lead Id

                    // Now you have the Lead Id, so you can set it for LeadDays
                    if (leadVm.LeadDay != null)
                    {
                        foreach (var leadDay in leadVm.LeadDay)
                        {
                            leadDay.LeadId = obj.Id; // Set the LeadId to the newly generated Lead's Id
                            _unitOfWork.LeadDay.Add(leadDay);
                        }
                    }
                }
                else
                {
                    _unitOfWork.Lead.Update(obj);

                    // Fetch existing LeadDays for the Lead
                    var existingLeadDays = _unitOfWork.LeadDay.GetAll(ld => ld.LeadId == obj.Id).ToList();

                    // Create a dictionary to track which LeadDays to update
                    var leadDaysToUpdate = new Dictionary<int, LeadDay>();

                    // Process the LeadDays in the view model
                    if (leadVm.LeadDay != null)
                    {
                        foreach (var leadDay in leadVm.LeadDay)
                        {
                            leadDay.LeadId = obj.Id; // Set the LeadId to match the Lead being updated

                            // Check if the LeadDay with the same Id already exists
                            var existingLeadDay = existingLeadDays.FirstOrDefault(ld => ld.Id == leadDay.Id);

                            if (existingLeadDay != null)
                            {
                                // Update the existing LeadDay with the new data
                                existingLeadDay.Destination = leadDay.Destination;
                                existingLeadDay.numOfDays = leadDay.numOfDays;
                                leadDaysToUpdate.Add(existingLeadDay.Id, existingLeadDay);
                            }
                            else
                            {
                                // Add the LeadDay as it's new
                                _unitOfWork.LeadDay.Add(leadDay);
                            }
                        }
                    }

                    // Remove LeadDays that were marked for deletion
                    var deleteLeadDayIds = Request.Form["deleteLeadDay"];
                    if (deleteLeadDayIds.Count > 0)
                    {
                        foreach (var deleteLeadDayId in deleteLeadDayIds)
                        {
                            if (int.TryParse(deleteLeadDayId, out int idToDelete))
                            {
                                var leadDayToDelete = existingLeadDays.FirstOrDefault(ld => ld.Id == idToDelete);
                                if (leadDayToDelete != null)
                                {
                                    _unitOfWork.LeadDay.Remove(leadDayToDelete);
                                }
                            }
                        }
                    }
                }

                // Handle LeadDays


                if (obj.TripStartDate != DateTime.MinValue)
                {
                    // Calculate the time 12 hours before the trip starts
                    var sendTime = obj.TripStartDate.AddHours(-12);
                    var sendTime2 = obj.TripStartDate.AddHours(-6);
                    var sendTime3 = obj.TripStartDate.AddMinutes(-2);
                    if (sendTime > DateTime.Now || sendTime3 > DateTime.Now)
                    {
                        // Get the driver's phone number from the 'Driver' property of the Lead
                        if (obj.Driver != null && !string.IsNullOrEmpty(obj.Driver.PhoneNumber))
                        {
                            var serviceProvider = HttpContext.RequestServices;
                            var smsReminderService = serviceProvider.GetRequiredService<SmsReminderService>();

                            // Schedule a new SMS reminder job with the updated sendTime
                            BackgroundJob.Schedule(() => smsReminderService.SendReminderToDriver(obj.Driver.PhoneNumber, sendTime), sendTime);
                            BackgroundJob.Schedule(() => smsReminderService.SendReminderToDriver(obj.Driver.PhoneNumber, sendTime2), sendTime2);
                            BackgroundJob.Schedule(() => smsReminderService.SendReminderToDriver(obj.Driver.PhoneNumber, sendTime3), sendTime3);
                        }
                    }
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

                if (obj.Status.ToString().ToLower() == "finished")
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

                        return RedirectToAction("SendSms", "Sms", new { reviewLink = reviewUrl, phoneNumber = phoneNum });
                    }
                }

                _unitOfWork.Save();
                return RedirectToAction("Leads");
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
