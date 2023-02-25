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
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(pm => pm.Match)
            .WithMany(m => m.Participants)
            .HasForeignKey(pm => pm.MatchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}