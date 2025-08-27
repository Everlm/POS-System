
using QuestPDF.Infrastructure;

namespace POS.Application.Documents
{
    public interface IDocumentGenerator
    {
        byte[] GeneratePdf(IDocument document);
        string GeneratePdfBase64(IDocument document);
        Task<byte[]> GeneratePdfAsync(IDocument document, CancellationToken ct = default);
        Task<string> GeneratePdfBase64Async(IDocument document, CancellationToken ct = default);
    }
}