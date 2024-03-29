﻿namespace POS.Domain.Entities
{
    public partial class Sale :BaseEntity
    {
        public Sale()
        {
            SaleDetails = new HashSet<SaleDetail>();
        }

        public int? ClientId { get; set; }
        public int? UserId { get; set; }
        public DateTime? SaleDate { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Total { get; set; }
       
        public virtual Client? Client { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
