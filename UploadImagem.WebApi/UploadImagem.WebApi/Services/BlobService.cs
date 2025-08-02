using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace UploadImagem.WebApi.Services
{
    public class BlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];

            var blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Garante que o contêiner exista
            _containerClient.CreateIfNotExists(PublicAccessType.None);
        }

        public async Task<string> UploadAsync(IFormFile arquivo)
        {
            var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(arquivo.FileName);
            var blobClient = _containerClient.GetBlobClient(nomeArquivo);

            var contentType = ObterTipoMime(arquivo.FileName);

            var headers = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = contentType
            };

            using var stream = arquivo.OpenReadStream();
            await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
            {
                HttpHeaders = headers
            });

            return blobClient.Uri.ToString(); // Retorna a URL pública do blob (caso o contêiner seja público)
        }

        public async Task<bool> DeleteAsync(string nomeArquivo)
        {
            var blobClient = _containerClient.GetBlobClient(nomeArquivo);
            return await blobClient.DeleteIfExistsAsync();
        }

        private string ObterTipoMime(string fileName)
        {
            var extensao = Path.GetExtension(fileName).ToLower();
            return extensao switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}
