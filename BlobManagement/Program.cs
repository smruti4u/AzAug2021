using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobManagement
{
    class Program
    {
        // Create Container
        // Create Blob / Upload Blob
        // List Blob
        // Update Metadata
        static async Task Main(string[] args)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=azaugsanew;AccountKey=Wz9bG487G6EMviJIr+gLzVSm5AYTfO7o/9s1FKV3HRIbbBL9cTHSiMubgUGlxaJSCmhYmN1ihKla/ANjPaqVxQ==;EndpointSuffix=core.windows.net";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            
            BlobContainerClient blobConainterClient = blobServiceClient.GetBlobContainerClient("vscontainer");
            blobConainterClient.CreateIfNotExists(PublicAccessType.BlobContainer);

            


            string fileName = "download.png";

            BlobClient blobClient = blobConainterClient.GetBlobClient($"vsfolder/{fileName}");
            await blobClient.UploadAsync(fileName, overwrite: true);

            // Result Segments, Segment Size , Pagination, CancellationToken
            var blobs = blobConainterClient.GetBlobs(prefix: "vsfolder");
            foreach (var blob in blobs)
            {
                Console.WriteLine(blob.Name);
            }


            // Adding Metadata
            IDictionary<string, string> metadata = new Dictionary<string, string>();
            metadata["author"] = "VisualStudio";
            metadata["subscriptionMode"] = "trial";
            blobClient.SetMetadata(metadata);


            HandleConCurrency(ConcurrencyType.Default, blobClient);            
            HandleConCurrency(ConcurrencyType.Optimistic, blobClient);
            HandleConCurrency(ConcurrencyType.Pessimistic, blobClient);

            Console.WriteLine("Blob upload is completed");
            Console.Read();
        }

        public static void HandleConCurrency(ConcurrencyType type, BlobClient blobClient)
        {
           var blobProperties =  blobClient.GetProperties();
            IDictionary<string, string> metadata = new Dictionary<string, string>();
            switch (type)
            {
                case ConcurrencyType.Default:
                    
                    metadata["author"] = "VisualStudio";
                    metadata["Concurrency"] = "Default";
                    blobClient.SetMetadata(metadata);
                    break;
                case ConcurrencyType.Optimistic:
                    BlobRequestConditions blobRequestConditions = new BlobRequestConditions()
                    {
                        IfMatch = blobProperties.Value.ETag
                    };
                    try
                    {
                        metadata["author"] = "VisualStudio";
                        metadata["Concurrency"] = "Optimistic";
                        blobClient.SetMetadata(metadata, blobRequestConditions);
                    }
                    catch(Exception exe)
                    {

                    }
                    break;
                case ConcurrencyType.Pessimistic:
                    var blobLeaseClient = blobClient.GetBlobLeaseClient();
                    var lease = blobLeaseClient.Acquire(TimeSpan.FromSeconds(30));
                    BlobRequestConditions blobRequestConditions1 = new BlobRequestConditions()
                    {
                        LeaseId = lease.Value.LeaseId
                    };
                    try
                    {
                        metadata["author"] = "VisualStudio";
                        metadata["Concurrency"] = "Pessimistic";
                        blobClient.SetMetadata(metadata, blobRequestConditions1);
                    }
                    catch (Exception exe)
                    {

                    }
            break;
            }
        }

    }
}
