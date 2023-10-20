using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Diagnostics;
using ZoomTourism.DataAccess.Repository;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models;
using ZoomTourism.Models.ViewModels;
using ZoomTourismWeb.Models;

namespace ZoomTourismWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;


        public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

     


        public IActionResult ReviewUpsert(string token)
        {
            var reviewLink = _unitOfWork.ReviewLink.GetFirstOrDefault(r => r.Token == token);

            if (reviewLink == null)
            {
                // Handle invalid or expired tokens (e.g., show an error message)
                return View("InvalidToken");
            }

            // Pass the LeadId to the view
            int leadId = reviewLink.LeadId;
            var reviewModel = new Review
            {
                LeadId = leadId
            };

            return View(reviewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReviewUpsert(Review obj)
        {
            if (ModelState.IsValid)
            {
                // Save the review to the database
                _unitOfWork.Review.Add(obj);
                _unitOfWork.Save();

                // Optionally, you can mark the review link as used to prevent further use
                var reviewLink = _unitOfWork.ReviewLink.GetFirstOrDefault(r => r.LeadId == obj.LeadId);
                if (reviewLink != null)
                {
                    reviewLink.IsUsed = true;
                    _unitOfWork.ReviewLink.Update(reviewLink);
                    _unitOfWork.Save();
                }

                return RedirectToAction("ThankYou"); // Redirect to a "Thank You" page
            }

            return View(obj);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}