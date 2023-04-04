using POS.Application.Dtos.PurchaseDetail;
using POS.Domain.Entities;

namespace POS.Application.Dtos.Purchase.Response
{
    public class PurchaseResponseDto
    {
        public int PurcharseId { get; set; }
        public string? Provider { get; set; }
        public string? User { get; set; }
        public DateTime? PurcharseDate { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Total { get; set; }
        public DateTime AuditCreateDate { get; set; }
        public int State { get; set; }
        public string? StatePurchase { get; set; }

        public virtual ICollection<PurchaseDetailDto> PurcharseDetails { get; set; }
    }
}
