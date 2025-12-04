using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Birahe.EndPoint.Entities.Configs;

public class PaymentConfig : IEntityTypeConfiguration<Payment> {
    public void Configure(EntityTypeBuilder<Payment> builder) {
        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(15);

        builder.Property(p => p.Authority)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.RefId)
            .HasMaxLength(200);
    }
}