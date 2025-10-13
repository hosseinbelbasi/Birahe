using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Birahe.EndPoint.Entities.Configs;

public class RiddleConfigs : IEntityTypeConfiguration<Riddle> {
    public void Configure(EntityTypeBuilder<Riddle> builder) {
        builder.HasIndex(x => x.RiddleUId).IsUnique();

        builder.Property(x => x.Content).HasMaxLength(1000);
        builder.Property(x => x.Department).HasMaxLength(70);
        builder.Property(x => x.HintImageFileName).HasMaxLength(500);
        builder.Property(x => x.RewardImageFileName).HasMaxLength(500);
        builder.Property(x => x.RiddleUId).HasMaxLength(80);
        builder.Property(x => x.Asnwer).HasMaxLength(2000);

        builder
            .Property(r => r.RiddleUId)
            .HasComputedColumnSql("[Department] + CAST([No] AS NVARCHAR(10))");

        builder.HasQueryFilter(r => r.RemoveTime.HasValue== false);

    }
}