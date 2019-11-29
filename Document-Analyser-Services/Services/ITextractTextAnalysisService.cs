using Amazon.Textract.Model;
using System.Threading.Tasks;

namespace Document_Analyzer_Services.Services
{
    public interface ITextractTextAnalysisService
    {
        Task<string> StartDocumentAnalysis(string bucketName, string key, string featureType);
        Task WaitForJobCompletion(string jobId, int delay = 5000);
        Task<GetDocumentAnalysisResponse> GetJobResults(string jobId);
    }
}
