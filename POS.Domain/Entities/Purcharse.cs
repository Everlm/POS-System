namespace POS.Domain.Entities
{
    public partial class Purcharse : BaseEntity
    {
        public int ProviderId { get; set; }
        public int WarehouseId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public string? Observation { get; set; }


        public virtual Provider Provider { get; set; } = null!;
        public virtual Warehouse Warehouse { get; set; } = null!;
        public virtual ICollection<PurcharseDetail> PurcharseDetails { get; set; } = null!;
    }
}
