using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Birahe.EndPoint.Entities.Configs;

public class StudentConfigs : IEntityTypeConfiguration<Student> {
    public void Configure(EntityTypeBuilder<Student> builder) {
        builder.HasIndex(x => x.StudentNo).IsUnique();

        builder.Property(x => x.FirstName).HasMaxLength(70);
        builder.Property(x => x.LastName).HasMaxLength(70);
        builder.Property(x => x.StudentNo).HasMaxLength(25);
        builder.Property(x => x.Field).HasMaxLength(70);

        builder.HasQueryFilter(x => !x.User.IsBanned);
    }
}