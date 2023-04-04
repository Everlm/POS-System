namespace POS.Domain.Entities
{
    public partial class Department : BaseEntity
    {
        public Department()
        {
            Provinces = new HashSet<Province>();
        }
        public string Name { get; set; } = null!;

        public virtual ICollection<Province> Provinces { get; set; }
    }
}


