using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Images.Data;
using Images.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Images.web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=UploadImages; Integrated Security=true;";

        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(IFormFile image, string password)
        {
            string fileName = $"{Guid.NewGuid()}-{image.FileName}";
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            image.CopyTo(fs);

            ImagesRepository repo = new ImagesRepository(_connectionString);
            int id = repo.AddImage(fileName, password);
            UploadViewModel vm = new UploadViewModel { Id = id, Password = password };
            return View(vm);
        }

        public IActionResult ViewImage(int imageId)
        {
            ImagesRepository repo = new ImagesRepository(_connectionString);

            bool canProceed = false;
            List<int> idsVerified = HttpContext.Session.Get<List<int>>("IdList");

            if (idsVerified!=null && idsVerified.Contains(imageId))
            {
                canProceed = true;
                repo.UpdateViews(imageId);
            }
            Image image = repo.GetImage(imageId);

            ViewImageViewModel vm = new ViewImageViewModel { CanProceed = canProceed, Image = image, Message = (string)TempData["Message"] };
            return View(vm);
        }
        public IActionResult VerifyPassword(int imageId, string passwordReal, string passwordTry)
        {
            ImagesRepository repo = new ImagesRepository(_connectionString);
            List<int> idsVerified = HttpContext.Session.Get<List<int>>("IdList");
            if (idsVerified == null)
            {
                idsVerified = new List<int>();
            }
            if (passwordReal == passwordTry)
            {
                idsVerified.Add(imageId);
                HttpContext.Session.Set("IdList", idsVerified);

            }
            else
            {

                TempData["Message"] = "Invalid password";

            }

            return Redirect($"/home/viewImage?imageId={imageId}");
        }







    }
}
