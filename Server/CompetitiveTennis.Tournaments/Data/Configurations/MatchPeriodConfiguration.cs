namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static DataConstants;

public class MatchPeriodConfiguration : IEntityTypeConfiguration<MatchPeriod>
{
    public void Configure(EntityTypeBuilder<MatchPeriod> builder)
    {
        builder
            .HasKey(s => s.Id);

        builder
            .Property(mp => mp.Set)
            .HasColumnType("smallint")
            .IsRequired();

        builder
            .Property(mp => mp.Game)
            .HasColumnType("smallint")
            .IsRequired();

        builder
            .Property(mp => mp.Status)
            .HasColumnType(CustomDbTypes.EventStatusEnum)
            .IsRequired();
        
        builder
            .Property( mp=> mp.Server)
            .HasColumnType(CustomDbTypes.EventActorEnum)
            .HasDefaultValueSql("unknown")
            .IsRequired();

        builder
            .HasOne(mp => mp.Match)
            .WithMany(m => m.Periods)
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
        
        builder
            .HasIndex(mp => new {mp.MatchId, mp.Set, mp.Game})
            .IsUnique();
            
        builder.HasIndex(mp => mp.MatchId);
    }
}