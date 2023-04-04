using POS.Application.Dtos.PurchaseDetail;

namespace POS.Application.Dtos.Purchase.Request
{
    public class PurchaseRequestDto
    {
        public int? ProviderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? PurcharseDate { get; set; }
        public decimal? Tax { get; set; }
        public int State { get; set; }
        public virtual ICollection<PurchaseDetailDto> PurcharseDetails { get; set; }
    }
}