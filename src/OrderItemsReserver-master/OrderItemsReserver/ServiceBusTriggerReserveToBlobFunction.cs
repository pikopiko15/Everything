using System;
using Azure.Storage.Blobs;
using System.IO;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrderItemsReserver
{
    public class ServiceBusTriggerReserveToBlobFunction
    {
        [FunctionName("ServiceBusTriggerReserveToBlob")]
        public void Run([ServiceBusTrigger("***", Connection = "***")]string queueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {queueItem}");

            try
            {
                // Handle errors test by sending the email through Logic Apps
                // throw new Exception("Test Exception");

                string blobContainerName = "***";

                // Storage account / Access keys / key1 -> Connection string
                string storageConfig = "***";               

                BlobServiceClient blobServiceClient = new BlobServiceClient(storageConfig);

                // Get the container (folder) the file will be saved in
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

                string blobName = blobContainerName + DateTime.UtcNow.ToShortTimeString();

                // Get the Blob Client used to interact with (including create) the blob
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                Azure.Response<BlobContentInfo> uploadResult;

                for (int i = 0; i < 3; i++)
                {
                    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(queueItem)))
                    {
                        uploadResult = blobClient.Upload(ms);
                    }

                    if (uploadResult.GetRawResponse().ReasonPhrase == "Created")
                    { return; }
                }
            }
            catch
            {
                PostHttpMessageAsync(queueItem);
            }
        }

        private async Task PostHttpMessageAsync(string message)
        {
            HttpClient httpclient = new HttpClient();

            // Logic app / Logic app designer / When a HTTP request is received panel -> HTTP POST URL
            var requestUri = "***";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httpRequestMessage.Content = new StringContent(message);
            HttpResponseMessage httpResponseMessage = await httpclient.SendAsync(httpRequestMessage);
        }
    }
}
