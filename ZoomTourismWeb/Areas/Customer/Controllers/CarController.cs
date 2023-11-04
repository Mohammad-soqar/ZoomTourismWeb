using Microsoft.AspNetCore.Mvc;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models.ViewModels;
using ZoomTourism.Models;

namespace ZoomTourismWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CarController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;


        public CarController(IUnitOfWork unitOfWork, ILogger<HomeController> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

        }

        public IActionResult CarRental()
        {
            IEnumerable<Car> CarList = _unitOfWork.Car.GetAll();
            return View(CarList);
        }
        public IActionResult CarDetails(int id)
        {
            IEnumerable<Car> CarList = _unitOfWork.Car.GetAll();

            if (id == 0)
            {
                return NotFound();
            }

            var car = _unitOfWork.Car.GetFirstOrDefault(u => u.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            var carImages = _unitOfWork.CarImage.GetAll(c => c.CarId == id);

            var carVM = new CarVM
            {
                Car = car,
                Images = carImages.ToList(),
                Cars = CarList
            };

            return View(carVM);
        }
    }
}
