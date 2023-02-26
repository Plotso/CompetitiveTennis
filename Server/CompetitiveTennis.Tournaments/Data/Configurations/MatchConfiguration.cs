namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static DataConstants;

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
            .Property( m=> m.Stage)
            .HasColumnType(CustomDbTypes.TournamentStageEnum)
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

        builder
            .HasOne(m => m.Tournament)
            .WithMany(t => t.Matches)
            .HasForeignKey(m => m.TournamentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(m => m.TournamentId);
    }
}