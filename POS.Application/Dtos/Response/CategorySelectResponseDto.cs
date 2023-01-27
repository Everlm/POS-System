using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Dtos.Response
{
    public class CategorySelectResponseDto
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
    }
}
