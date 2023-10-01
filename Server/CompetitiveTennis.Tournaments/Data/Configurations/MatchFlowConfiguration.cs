namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class MatchFlowConfiguration : IEntityTypeConfiguration<MatchFlow>
{
    public void Configure(EntityTypeBuilder<MatchFlow> builder)
    {
        builder
            .HasKey(mf => mf.Id);

        builder
            .Property(mf => mf.CreatedOn)
            .HasColumnType("timestamp")
            .IsRequired();

        builder
            .Property(mf => mf.ModifiedOn)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder
            .Property(mf => mf.DeletedOn)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(mf => mf.MatchId).IsRequired();
        builder.Property(mf => mf.SuccessorMatchId).IsRequired();

        builder
            .HasOne(m => m.Tournament)
            .WithMany(t => t.MatchFlows)
            .HasForeignKey(m => m.TournamentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(mf => mf.TournamentId);
        builder.HasIndex(t => new {t.MatchId, t.SuccessorMatchId}).IsUnique();
    }
}