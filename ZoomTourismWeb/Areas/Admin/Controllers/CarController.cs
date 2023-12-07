
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models;
using ZoomTourism.Models.ViewModels;
using System.Linq;
using System.Collections.Generic;
using ZoomTourism.Utility;
using Microsoft.AspNetCore.Authorization;
using Amazon.S3.Model;
using Amazon.S3;
using Amazon;

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

                    if (file != null)
                    {
                        var imageUrl = HandleImageUploadToS3(file, "CarThumb");
                        obj.Car.CarCardImage = imageUrl;
                    }

                    _unitOfWork.Car.Update(obj.Car);
                    _unitOfWork.Save();

                    if (images != null && images.Count > 0)
                    {
                        foreach (var image in images)
                        {
                            var imageUrl = HandleImageUploadToS3(image, "Cars");

                            var carImage = new CarImages
                            {
                                CarId = obj.Car.Id,
                                ImageUrl = imageUrl
                            };

                            _unitOfWork.CarImage.Add(carImage);
                            _unitOfWork.Save();
                        }
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    if (file != null)
                    {
                        var imageUrl = HandleImageUploadToS3(file, "CarThumb");
                        obj.Car.CarCardImage = imageUrl;
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
                            var imageUrl = HandleImageUploadToS3(image, "Cars");

                            var carImage = new CarImages
                            {
                                CarId = obj.Car.Id,
                                ImageUrl = imageUrl
                            };

                            _unitOfWork.CarImage.Add(carImage);
                            _unitOfWork.Save();
                        }
                    }

                    return RedirectToAction("Index");
                }
            }

            return View(obj);
        }

        private string HandleImageUploadToS3(IFormFile image, string s3Folder)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var s3Key = $"{s3Folder}/{fileName}";

            using (var stream = image.OpenReadStream())
            {
                var s3Client = new AmazonS3Client("AKIA2VXAQTX6WKS7IIHB", "CGvzFn0noWJSAKMKbT7It2eNSxzuJk9ZwVS6N/Bo", RegionEndpoint.EUCentral1);
                var putRequest = new PutObjectRequest
                {
                    BucketName = "zoomtourismassets",
                    Key = s3Key,
                    InputStream = stream,
                    ContentType = image.ContentType,
                    CannedACL = S3CannedACL.PublicRead // Set ACL as needed
                };

                s3Client.PutObjectAsync(putRequest).Wait();
            }

            return $"https://zoomtourismassets.s3.eu-central-1.amazonaws.com/{s3Key}";
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
