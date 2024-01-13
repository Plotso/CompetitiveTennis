namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static DataConstants;

public class ScoreConfiguration : IEntityTypeConfiguration<Score>
{
    public void Configure(EntityTypeBuilder<Score> builder)
    {
        builder
            .HasKey(s => s.Id);

        builder
            .Property(s => s.Set)
            .HasColumnType("smallint")
            .IsRequired();

        builder
            .Property(s => s.Game)
            .HasColumnType("smallint")
            .IsRequired();

        builder
            .Property(s => s.Participant1Points)
            .HasMaxLength(MaxScorePointsValueLength);

        builder
            .Property(s => s.Participant2Points)
            .HasMaxLength(MaxScorePointsValueLength);

        builder
            .Property(s => s.Status)
            .HasColumnType(CustomDbTypes.EventStatusEnum)
            .IsRequired();
        builder
            .Property( m=> m.PointWinner)
            .HasColumnType(CustomDbTypes.MatchOutcomeEnum)
            .IsRequired(false);

        builder
            .HasOne(s => s.Match)
            .WithMany(m => m.Scores)
            .HasForeignKey(s => s.MatchId)
            .OnDelete(DeleteBehavior.Restrict)
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
            
        builder.HasIndex(s => s.MatchId);
    }
}