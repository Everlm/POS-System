namespace POS.Application.Dtos.Purcharse.Request
{
    public class PurcharseRequestDto
    {
        public int ProviderId { get; set; }
        public int WarehouseId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public string? Observation { get; set; }
        public ICollection<PurcharseDetailRequestDto> PurcharseDetails { get; set; } = null!;
    }
}
