namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class ParticipantMatchConfiguration : IEntityTypeConfiguration<ParticipantMatch>
{
    public void Configure(EntityTypeBuilder<ParticipantMatch> builder)
    {
        builder
            .HasKey(pm => new {pm.ParticipantId, pm.MatchId});

        builder
            .HasOne(pm => pm.Participant)
            .WithMany(p => p.Matches)
            .HasForeignKey(pm => pm.ParticipantId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(pm => pm.Match)
            .WithMany(m => m.Participants)
            .HasForeignKey(pm => pm.MatchId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .Property(pm => pm.Specifier)
            .IsRequired();

        builder
            .Property(pm => pm.CreatedOn)
            .HasColumnType("timestamp")
            .IsRequired();

        builder
            .Property(pm => pm.ModifiedOn)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder
            .Property(pm => pm.DeletedOn)
            .HasColumnType("timestamp")
            .IsRequired(false);
    }
}