using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Birahe.EndPoint.Entities.Configs;

public class DiscountConfigs : IEntityTypeConfiguration<Discount> {
    public void Configure(EntityTypeBuilder<Discount> builder) {
        builder.HasIndex(d => d.Key).IsUnique();

        builder.Property(d => d.Title).HasMaxLength(50);
    }
}