using Amazon.Textract;
using Amazon.Textract.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Document_Analyser_Services.Services
{
    public class TextractTextAnalysisService : ITextractTextAnalysisService
    {
        private readonly IAmazonTextract _textract;

        public TextractTextAnalysisService(IAmazonTextract textract)
        {
            _textract = textract;
        }

        public async Task<string> StartDocumentAnalysis(string bucketName, string key, string featureType)
        {
            var request = new StartDocumentAnalysisRequest();
            var s3Object = new S3Object
            {
                Bucket = bucketName,
                Name = key
            };

            request.DocumentLocation = new DocumentLocation
            {
                S3Object = s3Object
            };

            request.FeatureTypes = new List<string> { featureType };
            
            var response = await _textract.StartDocumentAnalysisAsync(request);
            return response.JobId;
        }

        public async Task WaitForJobCompletion(string jobId, int delay = 5000)
        {
            while (!await IsJobComplete(jobId))
            {
                await Wait(delay);
            }
        }

        public async Task<GetDocumentAnalysisResponse> GetJobResults(string jobId)
        {
            var response = await _textract.GetDocumentAnalysisAsync(new GetDocumentAnalysisRequest
            {
                JobId = jobId
            });

            //response.Wait();
            return response;
        }

        private async Task<bool> IsJobComplete(string jobId)
        {
            var response = await _textract.GetDocumentAnalysisAsync(new GetDocumentAnalysisRequest
            {
                JobId = jobId
            });
            
            //response.Wait();
            return !response.JobStatus.Equals(JobStatus.IN_PROGRESS);
        }

        private async Task Wait(int delay = 5000)
        {
            await Task.Delay(delay);
        }
    }
}
