using Amazon.Textract;
using Amazon.Textract.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Document_Analyzer_Services.Services
{
    public class TextractTextDetectionService : ITextractTextDetectionService
    {
        private readonly IAmazonTextract _textract;

        public TextractTextDetectionService(IAmazonTextract textract)
        {
            _textract = textract;
        }

        public async Task<string> StartDocumentTextDetection(string bucketName, string key)
        {
            var request = new StartDocumentTextDetectionRequest
            {
                DocumentLocation = new DocumentLocation
                {
                    S3Object = new S3Object
                    {
                        Bucket = bucketName,
                        Name = key
                    }
                }
            };

            var response = await _textract.StartDocumentTextDetectionAsync(request);

            return response.JobId;
        }

        public async Task WaitForJobCompletion(string jobId, int delay = 5000)
        {
            while (!await IsJobComplete(jobId))
            {
                await Wait(delay);
            }
        }

        public async Task<List<GetDocumentTextDetectionResponse>> GetJobResults(string jobId)
        {
            var result = new List<GetDocumentTextDetectionResponse>();
            var response = await _textract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
            {
                JobId = jobId
            });

            result.Add(response);

            var nextToken = response.NextToken;
            while (nextToken != null)
            {
                await Wait();
                response = await _textract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
                {
                    JobId = jobId,
                    NextToken = response.NextToken
                });

                result.Add(response);
                nextToken = response.NextToken;
            }

            return result;
        }

        private async Task<bool> IsJobComplete(string jobId)
        {
            var response = await _textract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
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
