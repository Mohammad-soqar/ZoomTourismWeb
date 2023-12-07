
using Amazon.S3.Model;
using Amazon.S3;
using ZoomTourism.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZoomTourism.DataAccess.Repository.IRepository;
using ZoomTourism.Models;
using ZoomTourism.Models.ViewModels;
using Amazon;

namespace ZoomTourism.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_CodyleSupport)]
    public class TripController : Controller
    {
     
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _HostEnvironment;

        public TripController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
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
            Trip trip = new Trip();

            if (Id == null || Id == 0)
            {
                return View(trip);
            }
            else
            {
                trip = _unitOfWork.Trip.GetFirstOrDefault(u => u.Id == Id);
                return View(trip);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Trip obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        var extension = Path.GetExtension(file.FileName);
                        var fileName = Guid.NewGuid().ToString() + extension;

                        // Use the AmazonS3Client to upload the file to S3
                        var s3Client = new AmazonS3Client("AKIA2VXAQTX6WKS7IIHB", "CGvzFn0noWJSAKMKbT7It2eNSxzuJk9ZwVS6N/Bo", RegionEndpoint.EUCentral1);
                        var putRequest = new PutObjectRequest
                        {
                            BucketName = "zoomtourismassets",
                            Key = "Images/Trips/" + fileName,
                            InputStream = memoryStream,
                            ContentType = file.ContentType,
                            CannedACL = S3CannedACL.PublicRead // Optional: Set appropriate ACL for your use case
                        };

                        await s3Client.PutObjectAsync(putRequest);

                        obj.ImageUrl = "https://zoomtourismassets.s3.eu-central-1.amazonaws.com/Images/Trips/" + fileName;
                    }
                }

                if (obj.Id == 0)
                {
                    _unitOfWork.Trip.Add(obj);
                }
                else
                {
                    _unitOfWork.Trip.Update(obj);
                }

                _unitOfWork.Save();
                return RedirectToAction("TripsDashboard", "Home");
            }

            return View(obj);
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
            var tripList = _unitOfWork.Trip.GetAll();
            return Json(new { data = tripList });
        }

        [HttpDelete]
        public IActionResult DeletePost(int? Id)
        {
            try
            {
                var obj = _unitOfWork.Trip.GetFirstOrDefault(u => u.Id == Id);

                if (obj == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }

                // Use the AmazonS3Client to delete the file from S3
                var s3Client = new AmazonS3Client("AKIA2VXAQTX6WKS7IIHB", "CGvzFn0noWJSAKMKbT7It2eNSxzuJk9ZwVS6N/Bo", RegionEndpoint.EUCentral1);
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = "zoomtourismassets",
                    Key = obj.ImageUrl
                };

                s3Client.DeleteObjectAsync(deleteRequest);

                _unitOfWork.Trip.Remove(obj);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Exception: {ex.Message}");

                // Return an error response
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }



        #endregion


    }
}
