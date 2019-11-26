﻿using Amazon.Textract;
using Document_Analyser_Services.Infrastructure.Configuration;
using Document_Analyser_Services.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Document_Analyser_Services.Services
{
    public class DocumentReadAnalyzeService : IDocumentReadAnalyzeService
    {
        private readonly ITextractTextAnalysisService _analysisService;
        private readonly ITextractTextDetectionService _detectionService;
        private readonly S3Settings _s3Settings;

        public DocumentReadAnalyzeService(ITextractTextAnalysisService analysisService, ITextractTextDetectionService detectionService, S3Settings s3Settings)
        {
            _analysisService = analysisService;
            _detectionService = detectionService;
            _s3Settings = s3Settings;
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
            var jobId = await _analysisService.StartDocumentAnalysis(_s3Settings.S3BucketName ?? string.Empty, documentKey, "TABLES");
            
            await _analysisService.WaitForJobCompletion(jobId);
            var results = await _analysisService.GetJobResults(jobId);

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
    }
}
