namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static Constants;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder
            .Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(MaxTeamNameLength);
    }
}