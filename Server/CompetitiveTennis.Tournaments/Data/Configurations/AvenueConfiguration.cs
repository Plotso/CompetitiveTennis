namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using static DataConstants.Avenues;

public class AvenueConfiguration : IEntityTypeConfiguration<Avenue>
{
    public void Configure(EntityTypeBuilder<Avenue> builder)
    {
        builder
            .HasKey(a => a.Id);

        builder
            .Property(a => a.Name)
            .HasMaxLength(MaxNameLength)
            .IsRequired();

        builder
            .Property(a => a.Location)
            .HasMaxLength(MaxLocationLength)
            .IsRequired();

        builder
            .Property(a => a.City)
            .HasMaxLength(MaxLocationLength)
            .IsRequired();

        builder
            .Property(a => a.Country)
            .HasMaxLength(MaxLocationLength)
            .IsRequired();
        
        builder
            .Property(a => a.Details)
            .HasMaxLength(MaxDetailsLength);
        
        builder.Property(a => a.Courts)
            .HasColumnType("jsonb");

        // Indexes
        builder
            .HasIndex(a => a.Name)
            .IsUnique();
        
        builder
            .HasIndex(a => new {a.Country, a.City, a.Location})
            .IsUnique();

        builder
            .Property(a => a.CreatedBy)
            .IsRequired();
    }
}