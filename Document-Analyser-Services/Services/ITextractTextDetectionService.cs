using Amazon.Textract.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Document_Analyser_Services.Services
{
    public interface ITextractTextDetectionService
    {
        Task<string> StartDocumentTextDetection(string bucketName, string key);
        Task WaitForJobCompletion(string jobId, int delay = 5000);
        Task<List<GetDocumentTextDetectionResponse>> GetJobResults(string jobId);
    }
}
