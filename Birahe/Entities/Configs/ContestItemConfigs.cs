using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Birahe.EndPoint.Entities.Configs;

public class ContestItemConfigs : IEntityTypeConfiguration<ContestItem> {
    public void Configure(EntityTypeBuilder<ContestItem> builder) {
        builder.HasIndex(ci => new { ci.UserId, ci.RiddleId }).IsUnique();


        builder.Property(ci => ci.LastAnswer).HasMaxLength(2000);
    }
}