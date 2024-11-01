using ApiServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiServer.Infrastructure.EntityConfiguration;

public class FlashcardSetEntityConfiguration : IEntityTypeConfiguration<FlashcardSet>
{
    public void Configure(EntityTypeBuilder<FlashcardSet> builder)
    {
        builder.ToTable("FlashcardSets");

        builder.Property(x => x.Id)
            .HasColumnName("FlashcardSetId")
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();
       
        builder.Property(x => x.Name)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
        
        builder.Property(x => x.UpdatedAt)
            .IsRequired();
    }
}