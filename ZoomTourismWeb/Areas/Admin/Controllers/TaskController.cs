
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
    [Authorize(Roles = SD.Role_Admin+ "," + SD.Role_CallCenter + "," + SD.Role_CodyleSupport + "," + SD.Role_Booking)]
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

        public IActionResult taskview(int id)
        {
            ATask task = _unitOfWork.ATask.GetFirstOrDefault(r => r.Id == id);
            return View(task);
        }



        public IActionResult Upsert(int? Id, string? userid)
        {
            TaskVM taskVM = new()
            {
                Task = new(),
            };


            // Load users for CallCenter role
            var allUsers = _userManager.Users;
            taskVM.UserList = new SelectList(allUsers, "Id", "UserName");


            if (Id == null || Id == 0)
            {
                if (!string.IsNullOrEmpty(userid))
                {
                    taskVM.Task.AssignedUserId = userid;
                }
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
                Notification notification = new Notification
                {
                    Title = "New Task Assigned to you",
                    TitleAR = "مهمة جديدة",
                    TitleTR = "Yeni görev",
                    Description = $"A new task with the following name has been added: {obj.Title}",
                    DescriptionAR = $"تمت إضافة مهمة جديدة بالاسم {obj.Title}",
                    DescriptionTR = $"Aşağıdaki adda yeni bir görev eklendi {obj.Title}",
                    Type = "TaskAdded", // You can customize the type as needed
                    AssignedUserId = obj.AssignedUserId,
                    AssignedUser = obj.AssignedUser,
                    IsRead = false,
                    taskLink = $"/Admin/Task/taskview/{obj.Id}",
                };

                _unitOfWork.Notification.Add(notification);
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

            return RedirectToAction("Index");

            return View(Id);

        }
        public IActionResult TaskStatusDone(int? Id)
        {
            var Task = _unitOfWork.ATask.GetFirstOrDefault(u => u.Id == Id);
            if(Task.TaskStatus.ToString().ToLower() != "done")
            {
                Task.TaskStatus = Models.TaskStatus.Done;
                _unitOfWork.Save();
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }

        public IActionResult TaskStatusToDo(int? Id)
        {
            var Task = _unitOfWork.ATask.GetFirstOrDefault(u => u.Id == Id);
            if (Task.TaskStatus.ToString().ToLower() != "todo")
            {
                Task.TaskStatus = Models.TaskStatus.ToDo;
                _unitOfWork.Save();
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }

        public IActionResult TaskStatusInProgress(int? Id)
        {
            var Task = _unitOfWork.ATask.GetFirstOrDefault(u => u.Id == Id);
            if (Task.TaskStatus.ToString().ToLower() != "inprogress")
            {
                Task.TaskStatus = Models.TaskStatus.InProgress;
                _unitOfWork.Save();
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }

        #endregion


    }
}
