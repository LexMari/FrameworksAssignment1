using ApiServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiServer.Infrastructure.EntityConfiguration;

public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        // Composite key
        builder.HasKey(c => new { c.FlashcardSetId, c.UserId });

        builder.Property(x => x.FlashcardSetId)
            .IsRequired();
        
        builder.Property(x => x.UserId)
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CommentText)
            .HasMaxLength(1000)
            .IsUnicode(false)
            .IsRequired();
    }
}