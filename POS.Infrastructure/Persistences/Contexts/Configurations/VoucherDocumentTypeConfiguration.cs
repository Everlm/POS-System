using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domain.Entities;

namespace POS.Infrastructure.Persistences.Contexts.Configurations
{
    public class VoucherDocumentTypeConfiguration : IEntityTypeConfiguration<VoucherDoumentType>
    {
        public void Configure(EntityTypeBuilder<VoucherDoumentType> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("VoucherDoumentTypeId");

            builder.Property(e => e.Description)
                .HasMaxLength(30)
                .IsUnicode(false);
        }
    }
}
