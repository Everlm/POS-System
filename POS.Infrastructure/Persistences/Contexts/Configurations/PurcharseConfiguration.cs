﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domain.Entities;

namespace POS.Infrastructure.Persistences.Contexts.Configurations
{
    public class PurcharseConfiguration : IEntityTypeConfiguration<Purcharse>
    {
        public void Configure(EntityTypeBuilder<Purcharse> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("PurcharseId");

            builder.Property(e => e.Tax).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Purcharses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Purcharse__UserI__5629CD9C");
        }
    }
}
