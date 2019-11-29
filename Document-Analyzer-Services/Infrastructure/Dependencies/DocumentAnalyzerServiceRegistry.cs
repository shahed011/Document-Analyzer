using Amazon.S3.Transfer;
using Document_Analyzer_Common.Helpers;
using Document_Analyzer_Services.Infrastructure.Configuration;
using Document_Analyzer_Services.Services;
using Lamar;

namespace Document_Analyzer_Services.Infrastructure.Dependencies
{
    public class DocumentAnalyzerServiceRegistry : ServiceRegistry
    {
        public DocumentAnalyzerServiceRegistry()
        {
            ForSingletonOf<S3Settings>().Use(x => DependenciesHelper.GetSettings<S3Settings>(x, "S3Settings"));

            ForSingletonOf<ITransferUtility>().Use<TransferUtility>();
            ForSingletonOf<IFileService>().Use<FileService>();
            ForSingletonOf<ITextractTextAnalysisService>().Use<TextractTextAnalysisService>();
            ForSingletonOf<ITextractTextDetectionService>().Use<TextractTextDetectionService>();
            ForSingletonOf<IDocumentReadAnalyzeService>().Use<DocumentReadAnalyzeService>();
        }
    }
}
