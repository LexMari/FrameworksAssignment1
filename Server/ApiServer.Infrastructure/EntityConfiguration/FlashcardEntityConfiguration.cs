using ApiServer.Domain.Entities;
using ApiServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiServer.Infrastructure.EntityConfiguration;

public class FlashcardEntityConfiguration : IEntityTypeConfiguration<FlashCard>
{
    public void Configure(EntityTypeBuilder<FlashCard> builder)
    {
        builder.ToTable("FlashCards");

        builder.Property(x => x.Id)
            .HasColumnName("FlashCardId")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FlashcardSetId)
            .IsRequired();

        builder.Property(x => x.Question)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Answer)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Difficulty)
            .HasConversion(
                x => x.ToString(),
                x => (Difficulty)Enum.Parse(typeof(Difficulty), x))
            .IsRequired();

        //Foreign key
        builder.HasOne(x => x.FlashcardSet)
            .WithMany(x => x.Cards)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}