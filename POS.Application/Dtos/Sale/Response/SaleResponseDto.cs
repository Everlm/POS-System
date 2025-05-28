namespace POS.Application.Dtos.Sale.Response
{
    public class SaleResponseDto
    {
        public int SaleId { get; set; }
        public string Customer { get; set; } = null!;
        public string VoucherNumber { get; set; } = null!;
        public string VoucherDescription { get; set; } = null!;
        public string Warehouse { get; set; } = null!;
        public string? Observation { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DateOfSale { get; set; }
    }
}
