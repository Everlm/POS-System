namespace POS.Application.Dtos.PurchaseDetail
{
    public class PurchaseDetailDto
    {
        public int? ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
