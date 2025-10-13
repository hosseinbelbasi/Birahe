using Birahe.EndPoint.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Birahe.EndPoint.Entities.Configs;

public class UserConfigs : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> builder) {
        builder.HasIndex(x => x.Username).IsUnique();

        builder.Property(x => x.Username).HasMaxLength(70);
        builder.Property(x => x.Passwordhashed).HasMaxLength(150);
        builder.Property(x => x.SerialNumber).HasMaxLength(100);
        builder.Property(x => x.BanReason).HasMaxLength(1000);

        builder.HasQueryFilter(x => x.RemoveTime.HasValue == false);
        builder.HasQueryFilter(x => x.IsBanned == false);



    }
}