using Birahe.EndPoint.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Birahe.EndPoint.Entities.Configs;

public class UserConfigs : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> builder) {
        builder.HasIndex(x => x.Username).IsUnique();

        builder.Property(x => x.Username).HasMaxLength(70);

       
    }
}