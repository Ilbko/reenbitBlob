using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using reenbitBlob.Controllers;
using System.Net.Http;
using System.Text;

namespace TestProject
{
    public class HomeControllerTest
    {
        [Fact]
        public async Task FileUploadActionAsync()
        {
            
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var logger = new Mock<ILogger<HomeController>>();

            HomeController homeController = new HomeController(logger.Object)
            {
                TempData = tempData
            };

            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");

            try
            {
                IActionResult fileUploadActionResult = await homeController.Index(new reenbitBlob.Models.UploadModel() { File = file, Email = "test@test.com" });
                
            } catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }
    }
}