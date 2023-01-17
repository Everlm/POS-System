using System;
using System.Collections.Generic;

namespace POS.Domain.Entities
{
    public partial class Provider
    {
        public Provider()
        {
            Products = new HashSet<Product>();
            Purcharses = new HashSet<Purcharse>();
        }

        public int ProviderId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; } = null!;
        public string? Address { get; set; }
        public string Phone { get; set; } = null!;
        public int State { get; set; }
        public int AuditCreateUser { get; set; }
        public DateTime AuditCreateDate { get; set; }
        public int? AuditUpdateUser { get; set; }
        public DateTime? AuditUpdateDate { get; set; }
        public int? AuditDeleteUser { get; set; }
        public DateTime? AuditDeleteDate { get; set; }

        public virtual DocumentType DocumentType { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Purcharse> Purcharses { get; set; }
    }
}
