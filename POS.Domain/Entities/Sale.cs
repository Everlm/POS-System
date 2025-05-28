namespace POS.Domain.Entities
{
    public partial class Sale : BaseEntity
    {
        public int ClientId { get; set; }
        public int VoucherDocumentTypeId { get; set; }
        public int WarehouseId { get; set; }
        public string VoucherNumber { get; set; } = null!;
        public string? Observation { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }

        public virtual ICollection<SaleDetail> SaleDetails { get; set; } = null!;
        public virtual VoucherDocumentType VoucherDocumentType { get; set; } = null!;
        public virtual Warehouse Warehouse { get; set; } = null!;
        public virtual Client Client { get; set; } = null!;

    }
}
