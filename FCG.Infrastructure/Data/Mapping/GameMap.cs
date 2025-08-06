using FCG.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Data.Mapping;

public class GameMap : IEntityTypeConfiguration<GameEntity>
{
    public void Configure(EntityTypeBuilder<GameEntity> builder)
    {
        builder.ToTable("Game");

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .HasColumnType("NVARCHAR")
            .IsRequired();

        builder.Property(p => p.Gender)
            .HasMaxLength(20)
            .HasColumnType("NVARCHAR")
            .IsRequired();

        builder.Property(p => p.Value)
            .HasColumnType("DECIMAL(18,2)")
            .IsRequired();
    }
}