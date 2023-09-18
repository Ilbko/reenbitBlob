using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.AspNetCore.Mvc;

namespace reenbitBlob.Controllers
{
    public class BlobStorageController
    {
        public async Task UploadFile(IFormFile file)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.AddUserSecrets<BlobStorageController>().Build();

            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(
                new StorageCredentials(
                    configuration.GetSection("azureCredentials")["accountName"],
                    configuration.GetSection("azureCredentials")["accountKey"]
                    ), true);

            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = cloudBlobClient.GetContainerReference(configuration.GetSection("azureInfo")["containerName"]);
            await container.CreateIfNotExistsAsync();

            var blob = container.GetAppendBlobReference(file.FileName);
            using (var stream = file.OpenReadStream())         
                await blob.UploadFromStreamAsync(stream);                
        }
    }
}
