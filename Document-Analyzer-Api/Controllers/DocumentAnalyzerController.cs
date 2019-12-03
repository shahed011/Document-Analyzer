using Document_Analyzer_Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Document_Analyzer_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentAnalyzerController : ControllerBase
    {
        private readonly IDocumentReadAnalyzeService _readAnalyzeService;
        private readonly IFileService _fileService;
        private readonly ILogger<DocumentAnalyzerController> _logger;

        public DocumentAnalyzerController(IDocumentReadAnalyzeService readAnalyzeService, IFileService fileService, ILogger<DocumentAnalyzerController> logger)
        {
            _readAnalyzeService = readAnalyzeService;
            _fileService = fileService;
            _logger = logger;
        }

        [HttpPost]
        [Route("analyze")]
        public async Task<IActionResult> Get(IFormFile file)
        {
            var fileKey = await _fileService.UploadFileAsync(file);
            var response = await _readAnalyzeService.ReadDocumentTable(fileKey);

            return Ok(response);
        }
    }
}
