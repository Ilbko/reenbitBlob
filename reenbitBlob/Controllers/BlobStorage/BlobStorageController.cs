using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace reenbitBlob.Controllers
{
    public class BlobStorageController
    {
        public async Task UploadFileAsync(IFormFile file, string email)
        {
            try
            {
                ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                IConfiguration configuration = configurationBuilder.AddUserSecrets<BlobStorageController>().Build();

                CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(
                    new StorageCredentials(
                        configuration.GetSection("azureCredentials")["accountName"],
                        configuration.GetSection("azureCredentials")["accountKey"]
                        ), true);

                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var container = cloudBlobClient.GetContainerReference(configuration.GetSection("azureInfo")["containerName"]);
                await container.CreateIfNotExistsAsync();

                var blob = container.GetAppendBlobReference(file.FileName);
                blob.Metadata["email"] = email;
                using (var stream = file.OpenReadStream())
                    await blob.UploadFromStreamAsync(stream);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
