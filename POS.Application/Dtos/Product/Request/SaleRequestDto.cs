using POS.Domain.Entities;

namespace POS.Application.Dtos.Product.Request
{
    public class SaleRequestDto
    {
        public int? ClientId { get; set; }
        public int? UserId { get; set; }
        public DateTime? SaleDate { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Total { get; set; }
        public int State { get; set; }      
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
