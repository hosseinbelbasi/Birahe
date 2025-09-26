using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Birahe.EndPoint.Entities.Configs;

public class RiddleConfigs : IEntityTypeConfiguration<Riddle> {
    public void Configure(EntityTypeBuilder<Riddle> builder) {
        builder.Property(x => x.Content).HasMaxLength(1000);
        builder.Property(x => x.Department).HasMaxLength(70);
        builder.Property(x => x.Hint).HasMaxLength(500);
        builder.Property(x => x.Reward).HasMaxLength(500);
    }
}