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
            .HasMaxLength(MaxDetailsLength)
            .IsRequired(false);
        
        builder.Property(a => a.Courts)
            .HasColumnType("jsonb")
            .IsRequired(false);

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

        builder
            .Property(t => t.CreatedOn)
            .HasColumnType("timestamp")
            .IsRequired();

        builder
            .Property(t => t.ModifiedOn)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder
            .Property(t => t.DeletedOn)
            .HasColumnType("timestamp")
            .IsRequired(false);
    }
}