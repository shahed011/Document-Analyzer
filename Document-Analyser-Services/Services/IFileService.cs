using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Document_Analyser_Services.Services
{
    public interface IFileService
    {
        public Task<string> UploadFileAsync(IFormFile file);
    }
}
