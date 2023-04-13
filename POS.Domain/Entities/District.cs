namespace POS.Domain.Entities
{
    public partial class District : BaseEntity
    {
        public District()
        {
            BranchOffices = new HashSet<BranchOffice>();
            Businesses = new HashSet<Business>();
        }

        public int ProvinceId { get; set; }
        public string Name { get; set; } = null!;

        public virtual Province Province { get; set; } = null!;
        public virtual ICollection<BranchOffice> BranchOffices { get; set; }
        public virtual ICollection<Business> Businesses { get; set; }
    }
}
