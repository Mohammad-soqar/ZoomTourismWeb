
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models;
using ZoomTourism.Models.ViewModels;
using System.Linq;
using System.Collections.Generic;
using CodyleOffical.Utility;
using Microsoft.AspNetCore.Authorization;

namespace ZoomTourism.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CarController : Controller
    {
     
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _HostEnvironment;

        public CarController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _HostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
         
            return View();
        }

        public IActionResult Upsert(int? Id)
        {
            CarVM carVM = new CarVM
            {
                Car = new Car(),
                Images = new List<CarImages>()
            };

            if (Id == null || Id == 0)
            {

                return View(carVM);
            }


            else
            {
             
                carVM.Car = _unitOfWork.Car.GetFirstOrDefault(u => u.Id == Id);
                var existingImageUrls = _unitOfWork.CarImage
        .GetAllServ(ci => ci.CarId == Id)
        .Select(ci => ci.ImageUrl)
        .ToList();

                carVM.Car.ExistingImageUrls = string.Join(",", existingImageUrls);
                return View(carVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CarVM obj, List<IFormFile> images, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (obj.Car.Id == 0)
                {
                   
                    _unitOfWork.Car.Add(obj.Car);
                    _unitOfWork.Save();

                   
                    string wwwRootPath = _HostEnvironment.WebRootPath;

                    if (images != null && images.Count > 0)
                    {
                        foreach (var image in images)
                        {
                            string fileName = Guid.NewGuid().ToString();
                            var uploads = Path.Combine(wwwRootPath, @"Images\Cars");
                            var extension = Path.GetExtension(image.FileName);

                            var imageUrl = HandleImageUpload(image, uploads, fileName, extension);

                            var carImage = new CarImages
                            {
                                CarId = obj.Car.Id, 
                                ImageUrl = imageUrl
                            };

                            // Save the car image to the database
                            _unitOfWork.CarImage.Add(carImage);
                            _unitOfWork.Save();
                        }
                    }

                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"Images\CarThumb");
                        var extension = Path.GetExtension(file.FileName);

                        if (obj.Car.CarCardImage != null)
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, obj.Car.CarCardImage.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }


                        }
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStreams);
                        }
                        obj.Car.CarCardImage = @"\Images\CarThumb\" + fileName + extension;
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    string wwwRootPath = _HostEnvironment.WebRootPath;

                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"Images\CarThumb");
                        var extension = Path.GetExtension(file.FileName);

                        if (obj.Car.CarCardImage != null)
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, obj.Car.CarCardImage.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }


                        }
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStreams);
                        }
                        obj.Car.CarCardImage = @"\Images\CarThumb\" + fileName + extension;

                    }
                    _unitOfWork.Car.Update(obj.Car);
                    _unitOfWork.Save();
                    if (images != null && images.Count > 0)
                    {
                        var objImages = _unitOfWork.CarImage.GetAll(u => u.CarId == obj.Car.Id);

                        foreach (var image in objImages)
                        {
                            var imagePath = Path.Combine(_HostEnvironment.WebRootPath, image.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }

                        _unitOfWork.CarImage.RemoveRange(objImages);


                        foreach (var image in images)
                        {
                            string fileName = Guid.NewGuid().ToString();
                            var uploads = Path.Combine(wwwRootPath, @"Images\Cars");
                            var extension = Path.GetExtension(image.FileName);

                            // Handle image upload
                            var imageUrl = HandleImageUpload(image, uploads, fileName, extension);

                            // Create a new CarImages instance and associate it with the new car using its ID
                            var carImage = new CarImages
                            {
                                CarId = obj.Car.Id, // Set the car's ID as the foreign key
                                ImageUrl = imageUrl
                            };

                            // Save the car image to the database
                            _unitOfWork.CarImage.Add(carImage);
                            _unitOfWork.Save();
                        }
                    }
               
                    return RedirectToAction("Index");
                }
            }

            return View(obj);
        }

        private string HandleImageUpload(IFormFile image, string uploadPath, string fileName, string extension)
        {
            var uniqueFileName = fileName + extension;
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            // Return the relative path to the uploaded image
            return @"\Images\Cars\" + uniqueFileName;
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
            var carList = _unitOfWork.Car.GetAll();
            return Json(new { data = carList });
        }

        [HttpDelete]

        public IActionResult DeletePost(int? Id)
        {
            var obj = _unitOfWork.Car.GetFirstOrDefault(u => u.Id == Id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var objImages = _unitOfWork.CarImage.GetAll(u => u.CarId == Id);

            foreach (var image in objImages)
            {
                var imagePath = Path.Combine(_HostEnvironment.WebRootPath, image.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _unitOfWork.CarImage.RemoveRange(objImages);
            _unitOfWork.Car.Remove(obj);
            _unitOfWork.Save();

            // Redirect to the index action after deleting the car
            return RedirectToAction("Index");
        }


        #endregion



    }
}
