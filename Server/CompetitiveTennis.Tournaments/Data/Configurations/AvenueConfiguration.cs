﻿namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using static DataConstants.Avenues;

public class AvenueConfiguration : IEntityTypeConfiguration<Avenue>
{
    public void Configure(EntityTypeBuilder<Avenue> builder)
    {
        builder
            .HasKey(a => a.Id);

        builder
            .Property(a => a.Name)
            .HasMaxLength(MaxNameLength)
            .IsRequired();

        builder
            .Property(a => a.Location)
            .HasMaxLength(MaxLocationLength)
            .IsRequired();
        
        builder
            .Property(a => a.Details)
            .HasMaxLength(MaxDetailsLength);
        
        builder.Property(a => a.Courts)
            .HasColumnType("jsonb");
    }
}