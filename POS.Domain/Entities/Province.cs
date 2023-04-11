namespace POS.Domain.Entities
{
    public partial class Province : BaseEntity
    {
        public Province()
        {
            Districts = new HashSet<District>();
        }

        public string Name { get; set; } = null!;
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<District> Districts { get; set; }
    }
}
