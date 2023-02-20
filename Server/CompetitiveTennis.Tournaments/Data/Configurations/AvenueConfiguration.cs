namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static Constants;

public class AvenueConfiguration : IEntityTypeConfiguration<Avenue>
{
    public void Configure(EntityTypeBuilder<Avenue> builder)
    {
        builder
            .HasKey(a => a.Id);

        builder
            .Property(a => a.Name)
            .HasMaxLength(MaxAvenueNameLength);

        builder
            .Property(a => a.Location)
            .HasMaxLength(MaxAvenueLocationLength);
        
        builder
            .Property(a => a.Details)
            .HasMaxLength(MaxAvenueDetailsLength);
    }
}