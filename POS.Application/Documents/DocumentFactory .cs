using POS.Application.Documents.Category;
using POS.Application.Dtos.Category.Response;
using QuestPDF.Infrastructure;

namespace POS.Application.Documents
{
    public class DocumentFactory : IDocumentFactory
    {
        public IDocument CreateCategoryDocument(IEnumerable<CategoryResponseDto> categories)
        {
            return new CategoryDocument(categories);
        }
    }
}