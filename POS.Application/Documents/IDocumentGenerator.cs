
using QuestPDF.Infrastructure;

namespace POS.Application.Documents
{
    public interface IDocumentGenerator
    {
        byte[] GeneratePdf(IDocument document);
        string GeneratePdfBase64(IDocument document);
    }
}