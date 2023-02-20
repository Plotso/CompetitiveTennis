namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using static Constants;

public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
{
    public void Configure(EntityTypeBuilder<Tournament> builder)
    {
        builder
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(MaxTournamentTitleLength);

        builder
            .Property(t => t.Rules)
            .IsRequired()
            .HasMaxLength(MaxTournamentRulesLength);

        builder
            .Property(t => t.Description)
            .HasColumnType("text")
            .IsRequired();

        builder
            .Property(t => t.Type)
            .HasColumnType(CustomDbTypes.TournamentTypeEnum)
            .IsRequired();

        builder
            .Property(t => t.Surface)
            .HasColumnType(CustomDbTypes.SurfaceEnum)
            .IsRequired();
        
        builder
            .Property(t => t.EntryFee)
            .HasColumnType("money");

        builder
            .Property(t => t.Prize)
            .HasColumnType("money");

        builder
            .Property(t => t.CourtsAvailable)
            .HasColumnType("smallint")
            .IsRequired();

        builder
            .Property(t => t.MinParticipants)
            .HasColumnType("smallint")
            .IsRequired();

        builder
            .Property(t => t.MaxParticipants)
            .HasColumnType("smallint")
            .IsRequired();

        builder
            .Property(t => t.MatchWonPoints)
            .HasColumnType("smallint")
            .IsRequired(false);

        builder
            .Property(t => t.SetWonPoints)
            .HasColumnType("smallint")
            .IsRequired(false);

        builder
            .Property(t => t.GameWonPoints)
            .HasColumnType("smallint")
            .IsRequired(false);

        builder
            .HasOne(t => t.Avenue)
            .WithMany(a => a.Tournaments)
            .HasForeignKey(t => t.AvenueId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}