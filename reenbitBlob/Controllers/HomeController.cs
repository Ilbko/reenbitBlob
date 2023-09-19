using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using reenbitBlob.Controllers.BlobStorage;
using reenbitBlob.Models;
using System.Diagnostics;

namespace reenbitBlob.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private SecretClient secretClient;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };

            secretClient = new SecretClient(new Uri(configuration["AzureKeyValutUrl"]), new DefaultAzureCredential(), options);
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

            try
            {
                var blobStorageController = BlobStorageSingleton.getInstance();
                await blobStorageController.UploadFileAsync(uploadModel.File, uploadModel.Email, secretClient);

                TempData["Message"] = "File successfully uploaded!";
            } catch (Exception ex)
            {
                TempData["Message"] = "Something went wrong." + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}