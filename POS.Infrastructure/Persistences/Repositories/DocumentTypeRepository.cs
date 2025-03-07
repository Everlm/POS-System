using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly POSContext _context;

        public DocumentTypeRepository(POSContext context)
        {
            _context = context;
        }

        async Task<IEnumerable<DocumentType>> IDocumentTypeRepository.ListDocumentTypes()
        {
            var documenttypes = await _context.DocumentTypes
                .Where(x => x.State == (int)StateTypes.Active)
                .AsNoTracking()
                .ToListAsync();

            return documenttypes;

        }
    }
}
