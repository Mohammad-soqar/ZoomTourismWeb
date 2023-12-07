
using ZoomTourism.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models;
using ZoomTourism.Models.ViewModels;

namespace ZoomTourism.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class NotificationController : Controller
    {
     
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _HostEnvironment;

        public NotificationController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment,
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _HostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

    
        public IActionResult NotificationDetails(int id)
        {
            Notification Notification = _unitOfWork.Notification.GetFirstOrDefault(r => r.Id == id);
            if(Notification.IsRead == false)
            {
                
                Notification.IsRead = true;
                _unitOfWork.Notification.Update(Notification);
                _unitOfWork.Save();


            }
            return View(Notification);

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
            var NotificationList = _unitOfWork.Notification.GetAll();
            return Json(new { data = NotificationList });
        }

        [HttpDelete]

        public IActionResult DeletePost(int? Id)
        {
            var obj = _unitOfWork.Notification.GetFirstOrDefault(u => u.Id == Id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

           

            _unitOfWork.Notification.Remove(obj);
            _unitOfWork.Save();

            return RedirectToAction("AdminDashboard", "Home");

            return View(Id);

        }
       

        #endregion


    }
}
