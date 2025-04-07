using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FileMoverFunction
{
    public static class FileMoverFunction
    {
        [FunctionName("FileMover")]
        public static async Task Run(
            // Trigger that a file move request has been received
            [ServiceBusTrigger("%InputQueueName%", Connection = "ServiceBusConnection", AutoCompleteMessages = true)]
            ServiceBusReceivedMessage message,

            // Output binding to publish notifications when files are moved
            [ServiceBus("%OutputTopicName%", Connection = "ServiceBusConnection")]
            IAsyncCollector<ServiceBusMessage> notificationTopic,

            // Source blob container binding
            [Blob("%SourceBlobContainer%", Connection = "SourceStorageAccount")]
            BlobContainerClient sourceContainer,

            // Destination blob container binding
            [Blob("%DestinationBlobContainer%", Connection = "DestinationStorageAccount")]
            BlobContainerClient destinationContainer,

            // Logger
            ILogger log)
        {
            log.LogInformation($"Processing file move request triggered by message: {message.MessageId}");

            try
            {
                // Get the blob path from the message body
                var blobPath = message.Body.ToString();
                log.LogInformation($"Processing blob: {blobPath}");

                // Get references to source and destination blobs
                var sourceBlob = sourceContainer.GetBlobClient(blobPath);

                // Check if source blob exists
                if (!await sourceBlob.ExistsAsync())
                {
                    log.LogWarning($"Source blob doesn't exist: {blobPath}");
                    return;
                }

                // Create a destination path (could use a different naming strategy if needed)
                var destinationPath = $"{DateTime.UtcNow:yyyy-MM-dd}/{blobPath}";
                var destinationBlob = destinationContainer.GetBlockBlobClient(destinationPath);

                // Download from source and upload to destination
                var blobStream = await sourceBlob.OpenReadAsync();
                await destinationBlob.UploadAsync(blobStream);
                log.LogInformation($"Successfully copied blob to {destinationPath}");

                // Send notification about the moved blob
                await notificationTopic.AddAsync(new ServiceBusMessage(
                    $"{{\"sourcePath\":\"{blobPath}\",\"destinationPath\":\"{destinationPath}\",\"timestamp\":\"{DateTime.UtcNow:o}\"}}"));

                // Delete source blob
                await sourceBlob.DeleteIfExistsAsync();
                log.LogInformation($"Deleted source blob: {blobPath}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error processing blob move request");
                throw; // Rethrow to let Azure Functions handle the failure
            }
        }
    }
}

