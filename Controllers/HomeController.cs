using Microsoft.AspNetCore.Mvc;
using reenbitBlob.Controllers.BlobStorage;
using reenbitBlob.Models;
using System.Diagnostics;

namespace reenbitBlob.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Index(UploadModel uploadModel)
        {
            if (!ModelState.IsValid)
            {
                return View(uploadModel);
            }

            var blobStorageController = BlobStorageSingleton.getInstance();
            await blobStorageController.UploadFileAsync(uploadModel.File, uploadModel.Email);

            TempData["Message"] = "File successfully uploaded!";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}