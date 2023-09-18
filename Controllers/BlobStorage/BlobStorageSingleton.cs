namespace reenbitBlob.Controllers.BlobStorage
{
    public sealed class BlobStorageSingleton
    {
        private BlobStorageSingleton() { }
        private static BlobStorageController _instance;

        public static BlobStorageController getInstance()
        {
            if (_instance == null)
                _instance = new BlobStorageController(); 

            return _instance; 
        }
    }
}
