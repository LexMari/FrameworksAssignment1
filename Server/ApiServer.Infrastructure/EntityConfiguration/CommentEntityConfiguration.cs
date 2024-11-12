using ApiServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiServer.Infrastructure.EntityConfiguration;

public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.Property(x => x.Id)
            .HasColumnName("CommentId")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CommentText)
            .HasMaxLength(500)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(x => x.AuthorId)
            .IsRequired();

        //Foreign key
        builder.HasOne(x => x.Author)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}