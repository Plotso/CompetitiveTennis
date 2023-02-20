namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder
            .HasKey(a => a.Id);
        
        builder
            .HasOne(p => p.Tournament)
            .WithMany(t => t.Participants)
            .HasForeignKey(p => p.TournamentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(); 
        // ToDo: Decide whether we need to delete participants when tournament is deleted or we may want to preserve them for match statistics

        builder
            .HasOne(p => p.Team)
            .WithMany(t => t.Participants)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);


    }
}