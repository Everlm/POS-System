namespace POS.Application.Dtos.Product.Request
{
    public class ProductRequestDto
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int Stock { get; set; }
        public string? Image { get; set; }
        public decimal SellPrice { get; set; }
        public int CategoryId { get; set; }
        public int ProviderId { get; set; }
        public int State { get; set; }
    }
}
