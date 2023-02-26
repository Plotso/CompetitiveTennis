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
            .HasOne(s => s.Match)
            .WithMany(m => m.Scores)
            .HasForeignKey(s => s.MatchId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
            
        builder.HasIndex(s => s.MatchId);
    }
}