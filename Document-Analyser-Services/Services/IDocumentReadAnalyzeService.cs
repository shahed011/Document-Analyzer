using System.Threading.Tasks;

namespace Document_Analyser_Services.Services
{
    public interface IDocumentReadAnalyzeService
    {
        Task<string> ReadDocumentText(string documentKey);
        Task<string> ReadDocumentTable(string documentKey);
    }
}
