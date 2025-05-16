namespace POS.Application.Dtos.Sale.Request;
public  class SaleRequestDto
{
    public int VoucherDocumentTypeId { get; set; }
    public int WarehouseId { get; set; }
    public int CustomerId { get; set; }
    public string VoucherNumber { get; set; } = null!;
    public string? Observation { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Tax { get; set; }
    public decimal TotalAmount { get; set; }
    public ICollection<SaleDetailRequestDto> SaleDetail { get; set; } = null!;
}
