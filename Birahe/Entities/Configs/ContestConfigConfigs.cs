using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Birahe.EndPoint.Entities.Configs;

public class ContestConfigConfigs : IEntityTypeConfiguration<ContestConfig> {
    public void Configure(EntityTypeBuilder<ContestConfig> builder) {
        builder.HasIndex(cc => cc.Key).IsUnique();


        builder.Property(cc => cc.message).HasMaxLength(30);
        builder.Property(cc => cc.message).HasMaxLength(30);
    }
}