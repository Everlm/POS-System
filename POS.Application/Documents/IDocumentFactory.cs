using POS.Application.Dtos.Category.Response;
using QuestPDF.Infrastructure;

namespace POS.Application.Documents
{
    public interface IDocumentFactory
    {
        IDocument CreateCategoryDocument(IEnumerable<CategoryResponseDto> categories);
    }
}