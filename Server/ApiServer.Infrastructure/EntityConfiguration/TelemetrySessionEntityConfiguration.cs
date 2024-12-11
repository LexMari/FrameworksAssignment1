using ApiServer.Domain.Entities;
using ApiServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiServer.Infrastructure.EntityConfiguration;

public class TelemetrySessionEntityConfiguration : IEntityTypeConfiguration<TelemetrySession>
{
    public void Configure(EntityTypeBuilder<TelemetrySession> builder)
    {
        builder.ToTable("TelemetrySessions");
        
        builder.Property(x => x.Id)
            .HasColumnName("TelemetrySessionId")
            .HasMaxLength(25)
            .IsRequired();
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Status)
            .HasConversion(
                x => x.ToString(),
                x => (TelemetrySessionStatus)Enum.Parse(typeof(TelemetrySessionStatus), x))
            .IsRequired();
        
        builder.Property(x => x.StartTimestamp)
            .IsRequired();
        
        builder.Property(x => x.EndTimestamp)
            .IsRequired(false);
        
        builder.Property(x => x.UserId)
            .IsRequired();
        
        builder.Property(x => x.FlashcardSetId)
            .IsRequired();
        
        // Foreign key references
        builder.HasOne(x => x.FlashcardSet)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.User)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}