using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Dtos.DocumentType.Response
{
    public class DocumentTypeResponseDto
    {
        public int DocumentTypeId { get; set; }
        public string? Abbreviation { get; set; }
    }
}
