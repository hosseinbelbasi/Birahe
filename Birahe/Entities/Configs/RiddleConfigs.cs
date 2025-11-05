using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Birahe.EndPoint.Entities.Configs;

public class RiddleConfigs : IEntityTypeConfiguration<Riddle> {
    public void Configure(EntityTypeBuilder<Riddle> builder) {
        builder.HasIndex(x => x.RiddleUId).IsUnique();

        builder.Property(x => x.Content).HasMaxLength(1000);
        builder.Property(x => x.Department).HasMaxLength(70);
        builder.Property(x => x.HintFileName).HasMaxLength(500);
        builder.Property(x => x.RewardFileName).HasMaxLength(500);
        builder.Property(x => x.RiddleUId).HasMaxLength(80);
        builder.Property(x => x.Answer).HasMaxLength(2000);
        builder.Property(x => x.HintMediaType).HasMaxLength(10);
        builder.Property(x => x.RewardMediaType).HasMaxLength(10);
        builder.Property(x => x.Format).HasMaxLength(200);

        builder
            .Property(r => r.RiddleUId)
            .HasComputedColumnSql("[Department] + CAST([No] AS NVARCHAR(10))");

        builder.HasQueryFilter(r => r.RemoveTime.HasValue == false);
    }
}