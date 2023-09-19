using Azure.Security.KeyVault.Secrets;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace reenbitBlob.Controllers
{
    public class BlobStorageController
    {
        public async Task UploadFileAsync(IFormFile file, string email, SecretClient secretClient)
        {
            try
            {
                var accountName = await secretClient.GetSecretAsync("azureAccountName");
                var accountKey = await secretClient.GetSecretAsync("azureAccountKey");
                var containerName = await secretClient.GetSecretAsync("azureContainerName");

                CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(
                    new StorageCredentials(
                        accountName.Value.Value,
                        accountKey.Value.Value
                        ), true);

                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var container = cloudBlobClient.GetContainerReference(containerName.Value.Value);
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
