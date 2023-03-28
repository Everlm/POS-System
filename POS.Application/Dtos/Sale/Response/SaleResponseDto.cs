using POS.Domain.Entities;

namespace POS.Application.Dtos.Sale.Response
{
    public class SaleResponseDto
    {
        public int? SaleId { get; set; }
        public string? Client { get; set; }
        public string? User { get; set; }
        public DateTime? SaleDate { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Total { get; set; }
        public DateTime AuditCreateDate { get; set; }
        public int State { get; set; }
        public string? StateSale { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
