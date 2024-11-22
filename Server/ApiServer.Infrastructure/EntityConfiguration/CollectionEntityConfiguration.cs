using ApiServer.Domain.Entities;
using ApiServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiServer.Infrastructure.EntityConfiguration;

public class CollectionEntityConfiguration : IEntityTypeConfiguration<Collection>
{
    public void Configure(EntityTypeBuilder<Collection> builder)
    {
        builder.ToTable("Collections");

        builder.Property(x => x.Id)
            .HasColumnName("CollectionId")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Comment)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();
        
        // Foreign keys
        builder
            .HasMany(x => x.FlashcardSets)
            .WithMany(x => x.Collections)
            .UsingEntity<CollectionFlashcard>();
    }
}