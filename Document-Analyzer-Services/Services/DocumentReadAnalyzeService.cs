using Amazon.Textract;
using Document_Analyzer_Services.Infrastructure.Configuration;
using Document_Analyzer_Services.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Document_Analyzer_Services.Services
{
    public class DocumentReadAnalyzeService : IDocumentReadAnalyzeService
    {
        private readonly ITextractTextAnalysisService _analysisService;
        private readonly ITextractTextDetectionService _detectionService;
        private readonly S3Settings _s3Settings;
        private readonly ILogger _logger;

        private readonly List<string> ColumnTextsToCheck = new List<string> { "amount", "transaction", "transactions", "refund", "refunds", "commission", "chargeback", "chargebacks" };

        public DocumentReadAnalyzeService(ITextractTextAnalysisService analysisService, ITextractTextDetectionService detectionService, S3Settings s3Settings, ILogger logger)
        {
            _analysisService = analysisService;
            _detectionService = detectionService;
            _s3Settings = s3Settings;
            _logger = logger;
        }

        public async Task<TextractDocument> GetTextractDocument(string documentKey)
        {
            var jobId = await _analysisService.StartDocumentAnalysis(_s3Settings.S3BucketName ?? string.Empty, documentKey, "TABLES");

            await _analysisService.WaitForJobCompletion(jobId);
            var results = await _analysisService.GetJobResults(jobId);

            if (results.JobStatus == JobStatus.FAILED)
            {
                return new TextractDocument(new Amazon.Textract.Model.GetDocumentAnalysisResponse());
            }

            return new TextractDocument(results);
        }

        public async Task<string> ReadDocumentText(string documentKey)
        {
            var jobId = await _detectionService.StartDocumentTextDetection(_s3Settings.S3BucketName ?? string.Empty, documentKey);

            await _detectionService.WaitForJobCompletion(jobId);
            var response = await _detectionService.GetJobResults(jobId);

            var stringBuilder = new StringBuilder();
            if (response != null && response.Count > 0)
            {
                foreach (var item in response)
                {
                    foreach (var block in item.Blocks)
                    {
                        if (block.BlockType == BlockType.LINE)
                        {
                            stringBuilder.Append(block.Text).Append("\t");
                        }
                    }

                    stringBuilder.Append(Environment.NewLine);
                }
            }

            return stringBuilder.ToString();
        }

        public async Task<string> ReadDocumentTable(string documentKey)
        {
            _logger.Information("Started analyzing document");

            var jobId = await _analysisService.StartDocumentAnalysis(_s3Settings.S3BucketName ?? string.Empty, documentKey, "TABLES");
            
            await _analysisService.WaitForJobCompletion(jobId);
            var results = await _analysisService.GetJobResults(jobId);

            _logger.Information("Finished analyzing document");

            if (results.JobStatus == JobStatus.FAILED)
            {
                return string.Empty;
            }

            var document = new TextractDocument(results);

            var stringBuilder = new StringBuilder();
            foreach (var page in document.Pages)
            {
                foreach (var table in page.Tables)
                {
                    var rowIndex = 0;
                    foreach (var row in table.Rows)
                    {
                        rowIndex++;

                        var cellIndex = 0;
                        foreach (var cell in row.Cells)
                        {
                            cellIndex++;
                            stringBuilder.Append($"Table [{rowIndex}][{cellIndex}] = {cell.Text}").Append(Environment.NewLine);
                        }
                    }
                }
            }

            return stringBuilder.ToString();
        }

        public async Task<Dictionary<string, double>> ReadDocumentData(string documentKey)
        {
            _logger.Information("Started analyzing document");

            var jobId = await _analysisService.StartDocumentAnalysis(_s3Settings.S3BucketName ?? string.Empty, documentKey, "TABLES");

            await _analysisService.WaitForJobCompletion(jobId);
            var results = await _analysisService.GetJobResults(jobId);

            _logger.Information("Finished analyzing document");

            if (results.JobStatus == JobStatus.FAILED)
            {
                return new Dictionary<string, double>();
            }

            var document = new TextractDocument(results);
            var result = new Dictionary<string, double>();

            var cellsToConsider = new List<int>();
            foreach (var page in document.Pages)
            {
                foreach (var table in page.Tables)
                {
                    cellsToConsider = new List<int>();
                    foreach (var row in table.Rows)
                    {
                        if (cellsToConsider.Any())
                        {
                            foreach (var cellIndex in cellsToConsider)
                            {
                                var key = table.Rows[0].Cells[cellIndex - 1].Text;
                                var valueString = row.Cells[cellIndex - 1].Text;

                                if (double.TryParse(valueString, out var value))
                                {
                                    if (result.ContainsKey(key))
                                        result[key] += value;
                                    else
                                        result.Add(key, value);
                                }
                            }

                            continue;
                        }

                        foreach (var cell in row.Cells)
                        {
                            if (cell.Text.Split(" ").Any(x => ColumnTextsToCheck.Contains(x.ToLower())) && !cellsToConsider.Any(x => x == cell.ColumnIndex))
                            {
                                cellsToConsider.Add(cell.ColumnIndex);
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
