namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static Constants;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder
            .Property( m=> m.Status)
            .HasColumnType(CustomDbTypes.EventStatusEnum)
            .IsRequired();
        
        builder
            .Property( m=> m.Outcome)
            .HasColumnType(CustomDbTypes.MatchOutcomeEnum)
            .IsRequired();

        builder
            .Property(m => m.MatchWonPoints)
            .HasColumnType("smallint")
            .IsRequired(false);

        builder
            .Property(m => m.SetWonPoints)
            .HasColumnType("smallint")
            .IsRequired(false);

        builder
            .Property(m => m.GameWonPoints)
            .HasColumnType("smallint")
            .IsRequired(false);
    }
}