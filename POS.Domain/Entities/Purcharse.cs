namespace POS.Domain.Entities
{
    public partial class Purcharse :BaseEntity
    {
        public Purcharse()
        {
            PurcharseDetails = new HashSet<PurcharseDetail>();
        }

        public int? ProviderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? PurcharseDate { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Total { get; set; }     

        public virtual Provider? Provider { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<PurcharseDetail> PurcharseDetails { get; set; }
    }
}
