namespace POS.Application.Dtos.Sale.Response
{
    public class SaleResponseDto
    {
        public int SaleId { get; set; }
        //TODO:Revisar es customer
        public string Client { get; set; } = null!;
        public string VoucherNumber { get; set; } = null!;
        public string VoucherDescription { get; set; } = null!;
        public string Warehouse { get; set; } = null!;
        public string? Observation { get; set; }
        public decimal TotalAmout { get; set; }
        public DateTime DateOfSale { get; set; }
    }
}
