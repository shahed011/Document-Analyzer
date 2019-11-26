using Amazon.S3.Transfer;
using Document_Analyser_Common.Helpers;
using Document_Analyser_Services.Infrastructure.Configuration;
using Document_Analyser_Services.Services;
using Lamar;

namespace Document_Analyser_Services.Infrastructure.Dependencies
{
    public class DocumentAnalyserServiceRegistry : ServiceRegistry
    {
        public DocumentAnalyserServiceRegistry()
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
