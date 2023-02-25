namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class AccountParticipantConfiguration : IEntityTypeConfiguration<AccountParticipant>
{
    public void Configure(EntityTypeBuilder<AccountParticipant> builder)
    {
        builder
            .HasKey(ap => new {ap.AccountId, ap.ParticipantId});

        builder
            .HasOne(ap => ap.Account)
            .WithMany(a => a.Participations)
            .HasForeignKey(ap => ap.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(ap => ap.Participant)
            .WithMany(p => p.Players)
            .HasForeignKey(ap => ap.ParticipantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}