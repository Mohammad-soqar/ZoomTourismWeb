// NotificationViewComponent.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ZoomTourism.DataAccess.Data;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models;



    

    public class NotificationViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _HostEnvironment;

        public NotificationViewComponent(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment,
            UserManager<IdentityUser> userManager)
        {
        _unitOfWork = unitOfWork;
        _HostEnvironment = hostEnvironment;
        _userManager = userManager;
    }

        public IViewComponentResult Invoke()
        {
        try
        {
            var notifications = _unitOfWork.Notification.GetAll().ToList();
            return View(notifications);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Exception in NotificationViewComponent: {ex.Message}");
            throw; // Rethrow the exception to maintain the original behavior
        }
    }
    }


