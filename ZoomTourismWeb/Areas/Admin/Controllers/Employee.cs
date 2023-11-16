using CodyleOffical.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models;
using ZoomTourism.Models.ViewModels;

namespace ZoomTourismWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_CodyleSupport)]

    public class Employee : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _HostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public Employee(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment,
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _HostEnvironment = hostEnvironment;
            _userManager = userManager;
        }
        public IActionResult Employees()
        {
            var users = _userManager.Users.ToList(); // Fetch users from the database
            var filteredUsers = users
                .Where(user => !_userManager.IsInRoleAsync(user, SD.Role_Customer).Result)
                .ToList(); // Filter out users with the "Customer" role

            var usersWithRolesAndNames = new List<UserVM>();

            foreach (var user in filteredUsers)
            {
                var roles = _userManager.GetRolesAsync(user).Result; // Blocking call

                var Id = user.Id;
                var ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == Id);
                var name = ApplicationUser?.Name;

                var userViewModel = new UserVM
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Name = name, // Replace with the actual property in your ApplicationUser model
                    Roles = roles.ToList()
                };

                usersWithRolesAndNames.Add(userViewModel);
            }

            return View(usersWithRolesAndNames);
        }






    }
}
