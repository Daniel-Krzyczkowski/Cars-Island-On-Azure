using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using CarsIsland.Infrastructure.Configuration.Interfaces;
using CarsIsland.Infrastructure.Services.Storage.Interfaces;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CarsIsland.Infrastructure.Services.Storage
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IBlobStorageServiceConfiguration _blobStorageServiceConfiguration;
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(IBlobStorageServiceConfiguration blobStorageServiceConfiguration,
                              BlobServiceClient blobServiceClient)
        {
            _blobStorageServiceConfiguration = blobStorageServiceConfiguration
                    ?? throw new ArgumentNullException(nameof(blobStorageServiceConfiguration));

            _blobServiceClient = blobServiceClient
                    ?? throw new ArgumentNullException(nameof(blobServiceClient));
        }

        public async Task DeleteBlobIfExistsAsync(string blobName)
        {
            try
            {
                var container = await GetBlobContainer();
                var blockBlob = container.GetBlobClient(blobName);
                await blockBlob.DeleteIfExistsAsync();
            }
            catch (RequestFailedException ex)
            {
                Log.Error($"Document {blobName} was not deleted successfully - error details: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DoesBlobExistAsync(string blobName)
        {
            try
            {
                var container = await GetBlobContainer();
                var blockBlob = container.GetBlobClient(blobName);
                var doesBlobExist = await blockBlob.ExistsAsync();
                return doesBlobExist.Value;
            }
            catch (RequestFailedException ex)
            {
                Log.Error($"Document {blobName} existence cannot be verified - error details: {ex.Message}");
                throw;
            }
        }

        public async Task DownloadBlobIfExistsAsync(Stream stream, string blobName)
        {
            try
            {
                var container = await GetBlobContainer();
                var blockBlob = container.GetBlobClient(blobName);

                await blockBlob.DownloadToAsync(stream);

            }

            catch (RequestFailedException ex)
            {
                Log.Error($"Cannot download document {blobName} - error details: {ex.Message}");
                if (ex.ErrorCode != "404")
                {
                    throw;
                }
            }
        }

        public async Task<string> GetBlobUrl(string blobName)
        {
            try
            {
                var container = await GetBlobContainer();
                var blob = container.GetBlobClient(blobName);

                string blobUrl = blob.Uri.AbsoluteUri;
                return blobUrl;
            }
            catch (RequestFailedException ex)
            {
                Log.Error($"Url for document {blobName} was not found - error details: {ex.Message}");
                throw;
            }
        }

        public async Task<string> UploadBlobAsync(Stream stream, string blobName)
        {
            try
            {
                Debug.Assert(stream.CanSeek);
                stream.Seek(0, SeekOrigin.Begin);
                var container = await GetBlobContainer();

                BlobClient blob = container.GetBlobClient(blobName);
                await blob.UploadAsync(stream);
                return blob.Uri.AbsoluteUri;
            }

            catch (RequestFailedException ex)
            {
                Log.Error($"Document {blobName} was not uploaded successfully - error details: {ex.Message}");
                throw;
            }
        }

        private async Task<BlobContainerClient> GetBlobContainer()
        {
            try
            {
                BlobContainerClient container = _blobServiceClient
                                .GetBlobContainerClient(_blobStorageServiceConfiguration.ContainerName);

                await container.CreateIfNotExistsAsync();

                return container;
            }
            catch (RequestFailedException ex)
            {
                Log.Error($"Cannot find blob container: {_blobStorageServiceConfiguration.ContainerName} - error details: {ex.Message}");
                throw;
            }
        }

        public string GenerateSasTokenForContainer()
        {
            BlobSasBuilder builder = new BlobSasBuilder();
            builder.BlobContainerName = _blobStorageServiceConfiguration.ContainerName;
            builder.ContentType = "video/mp4";
            builder.SetPermissions(BlobAccountSasPermissions.Read);
            builder.StartsOn = DateTimeOffset.UtcNow;
            builder.ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(90);
            var sasToken = builder
                        .ToSasQueryParameters(new StorageSharedKeyCredential(_blobStorageServiceConfiguration.AccountName,
                                                                             _blobStorageServiceConfiguration.Key))
                        .ToString();
            return sasToken;
        }
    }
}
