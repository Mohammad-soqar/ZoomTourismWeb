
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
    public class BlogController : Controller
    {
     
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _HostEnvironment;

        public BlogController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
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
            BlogVM BlogVM = new()
            {
                blog = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (Id == null || Id == 0)
            {
                return View(BlogVM);
            }
            else
            {
                BlogVM.blog = _unitOfWork.Blog.GetFirstOrDefault(u => u.Id == Id);
                return View(BlogVM);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(BlogVM obj, IFormFile file)
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
                            Key = "Images/Blogs/" + fileName,
                            InputStream = memoryStream,
                            ContentType = file.ContentType,
                            CannedACL = S3CannedACL.PublicRead // Optional: Set appropriate ACL for your use case
                        };

                        await s3Client.PutObjectAsync(putRequest);

                        obj.blog.ImageUrl = "https://zoomtourismassets.s3.eu-central-1.amazonaws.com/Images/Blogs/" + fileName;
                    }
                }
                if (obj.blog.Id == 0)
                {
                    _unitOfWork.Blog.Add(obj.blog);
                }
                else
                {
                    _unitOfWork.Blog.Update(obj.blog);
                }


                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(obj);

        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == Id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View();

        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var blogList = _unitOfWork.Blog.GetAll(includeProperties: "Category");
            return Json(new { data = blogList });
        }

        [HttpDelete]

        public IActionResult DeletePost(int? Id)
        {
            try
            {
                var obj = _unitOfWork.Blog.GetFirstOrDefault(u => u.Id == Id);

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

                _unitOfWork.Blog.Remove(obj);
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
