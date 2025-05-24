using FCG.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Data.Mapping;

public class UserMap : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("User");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .HasColumnType("NVARCHAR")
            .IsRequired();

        builder.OwnsOne(x => x.EmailAddress)
            .Property(x => x.Address)
            .HasColumnName("Email")
            .IsRequired(true);

        builder.OwnsOne(x => x.Password)
            .Property(x => x.Hash)
            .HasColumnName("PasswordHash")
            .IsRequired();
    }
}