
using CodyleOffical.Utility;
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
    public class TaskController : Controller
    {
     
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _HostEnvironment;

        public TaskController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment,
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _HostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            IEnumerable<ATask> userTasks = _unitOfWork.ATask.GetAll()
                    .Where(task => task.AssignedUserId == userId); 
            return View(userTasks);
        }

      


        public IActionResult Upsert(int? Id)
        {
            TaskVM taskVM = new()
            {
                Task = new()
            };


            // Load users for CallCenter role
            var allUsers = _userManager.Users;
            taskVM.UserList = new SelectList(allUsers, "Id", "UserName");


            if (Id == null || Id == 0)
            {
                return View(taskVM);
            }
            else
            {
                var Task = _unitOfWork.ATask.GetFirstOrDefault(u => u.Id == Id);
                taskVM.Task = Task;


                return View(taskVM);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(TaskVM taskVM)
        {
            if (ModelState.IsValid)
            {
                ATask obj = taskVM.Task;
                if (!string.IsNullOrEmpty(obj.AssignedUserId))
                {
                    // Assign Call Center user based on obj.CallCenterUserId
                    var assignedUser = _userManager.FindByIdAsync(obj.AssignedUserId).Result;
                    obj.AssignedUser = (ApplicationUser)assignedUser;
                }
                string wwwRootPath = _HostEnvironment.WebRootPath;
             
                if (obj.Id == 0)
                {
                    _unitOfWork.ATask.Add(obj);
                }
                else
                {
                    _unitOfWork.ATask.Update(obj);
                }


                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(taskVM);

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
            var ATaskList = _unitOfWork.ATask.GetAll();
            return Json(new { data = ATaskList });
        }

        [HttpDelete]

        public IActionResult DeletePost(int? Id)
        {
            var obj = _unitOfWork.ATask.GetFirstOrDefault(u => u.Id == Id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

           

            _unitOfWork.ATask.Remove(obj);
            _unitOfWork.Save();

            string script = "window.location.reload();";
            return Content("<script>" + script + "</script>");

            return View(Id);

        }


        #endregion


    }
}
