namespace POS.Application.Dtos.Purchase.Response
{
    public class PurcharseByIdResponseDto
    {
        public int PurchaseId { get; set; }
        public int ProviderId { get; set; }
        public int WarehouseId { get; set; }
        public string? Observation { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }

        public ICollection<PurcharseDetailByIdResponseDto> PurchaseDetail { get; set; } = null!;
    }
}
