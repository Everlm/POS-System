using POS.Application.Dtos.SaleDetails;

namespace POS.Application.Dtos.Sale.Request
{
    public class SaleRequestDto
    {
        public int? ClientId { get; set; }
        public int? UserId { get; set; }
        public DateTime? SaleDate { get; set; }
        public decimal? Tax { get; set; }
        //public decimal? Total { get; set; }
        public int State { get; set; }
        public virtual ICollection<SaleDetailDto> SaleDetails { get; set; }
    }
}
