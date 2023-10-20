using Microsoft.AspNetCore.Mvc;

namespace ZoomTourismWeb.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult AdminDashboard()
        {
            return View();
        }
        public IActionResult CallCenterDashboard()
        {
            return View();
        }

        public IActionResult BookingDepartmentDashboard()
        {
            return View();
        }
    }
}
