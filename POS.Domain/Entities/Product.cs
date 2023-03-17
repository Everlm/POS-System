namespace POS.Domain.Entities
{
    public partial class Product :BaseEntity
    {
        public Product()
        {
            PurcharseDetails = new HashSet<PurcharseDetail>();
            SaleDetails = new HashSet<SaleDetail>();
        }

        public string? Code { get; set; }
        public string? Name { get; set; }
        public int Stock { get; set; }
        public string? Image { get; set; }
        public decimal SellPrice { get; set; }
        public int CategoryId { get; set; }
        public int ProviderId { get; set; }     

        public virtual Category Category { get; set; } = null!;
        public virtual Provider Provider { get; set; } = null!;
        public virtual ICollection<PurcharseDetail> PurcharseDetails { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
