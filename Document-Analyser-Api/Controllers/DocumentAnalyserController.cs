using Document_Analyser_Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Document_Analyser_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentAnalyserController : ControllerBase
    {
        private readonly IDocumentReadAnalyzeService _readAnalyzeService;
        private readonly IFileService _fileService;
        private readonly ILogger<DocumentAnalyserController> _logger;

        public DocumentAnalyserController(IDocumentReadAnalyzeService readAnalyzeService, IFileService fileService, ILogger<DocumentAnalyserController> logger)
        {
            _readAnalyzeService = readAnalyzeService;
            _fileService = fileService;
            _logger = logger;
        }

        [HttpPost]
        [Route("analyse")]
        public async Task<IActionResult> Get(IFormFile file)
        {
            _logger.LogInformation("In Controller ------------------");
            var fileKey = await _fileService.UploadFileAsync(file);
            //var response = await _readAnalyzeService.ReadDocumentText(fileKey);
            var response = await _readAnalyzeService.ReadDocumentTable(fileKey);

            return Ok(response);
        }
    }
}
