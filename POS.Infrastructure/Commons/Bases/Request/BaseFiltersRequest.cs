using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Infrastructure.Commons.Bases.Request
{
    public class BaseFiltersRequest:BasePaginationRequest
    {
        public int? NumFilter { get; set; } = null;
        public string? TextFilter { get; set; } = null; 
        public int? StateFilter { get; set; } = null;
        public string? StartDate { get; set; } = null;
        public string? EndDate { get; set; } = null;
        public bool? Download { get; set; } = false;
    }
}
