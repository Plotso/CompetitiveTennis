namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using static DataConstants;

public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
{
    public void Configure(EntityTypeBuilder<Tournament> builder)
    {
        builder
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(Tournaments.MaxTitleLength);

        builder
            .Property(t => t.Rules)
            .IsRequired()
            .HasMaxLength(Tournaments.MaxRulesLength);

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
        
        builder
            .HasOne(t => t.Organiser)
            .WithMany(a => a.OrganisedTournaments)
            .HasForeignKey(t => t.OrganiserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasIndex(t => t.AvenueId);
        builder.HasIndex(t => t.OrganiserId);
    }
}