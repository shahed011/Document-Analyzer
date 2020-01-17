using Document_Analyzer_Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Document_Analyzer_Services.Services
{
    public interface IDocumentReadAnalyzeService
    {
        Task<TextractDocument> GetTextractDocument(string documentKey);
        Task<string> ReadDocumentText(string documentKey);
        Task<string> ReadDocumentTable(string documentKey);
        Task<Dictionary<string, double>> ReadDocumentData(string documentKey);
    }
}
