using Amazon.S3.Transfer;
using Document_Analyzer_Services.Infrastructure.Configuration;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Document_Analyzer_Services.Services
{
    public class FileService : IFileService
    {
        private readonly ITransferUtility _transferUtility;
        private readonly S3Settings _s3Settings;
        private readonly ILogger _logger;

        public FileService(ITransferUtility transferUtility, S3Settings s3Settings, ILogger logger)
        {
            _transferUtility = transferUtility;
            _s3Settings = s3Settings;
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            try
            {
                var fileKey = Guid.NewGuid().ToString();
                using var fileStream = file.OpenReadStream();

                await _transferUtility.UploadAsync(fileStream, _s3Settings.S3BucketName, fileKey);

                return fileKey;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to upload file to S3");

                return string.Empty;
            }
        }
    }
}
