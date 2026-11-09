using System.Threading.Tasks;


namespace RaceDay.Api.Services
{
    public interface IAzureBlobStorageService
    {
        public Task<string> UploadFileAsync(IFormFile file, string eventId);

        public string GetReadUrl(string blobUrl);
    }
}

