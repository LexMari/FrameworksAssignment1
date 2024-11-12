using ApiServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiServer.Infrastructure.EntityConfiguration;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(x => x.Id)
            .HasColumnName("UserId")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username)
            .HasMaxLength(50)
            .IsUnicode(false)
            .IsRequired();
                
        builder.HasAlternateKey(c => c.Username);

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(128)
            .IsRequired();
        
        builder.Property(x => x.PasswordSalt)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.IsAdministrator)
            .IsRequired();

        builder.HasData(
            new User(1, "student", "password", false),
            new User(2, "admin", "password", true));
    }
}