using ApiServer.Domain.Entities;
using ApiServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiServer.Infrastructure.EntityConfiguration;

public class ApiSettingEntityConfiguration : IEntityTypeConfiguration<ApiSetting>
{
    public void Configure(EntityTypeBuilder<ApiSetting> builder)
    {
        builder.ToTable("ApiSettings");

        builder.Property(x => x.Id)
            .HasColumnName("ApiSettingId")
            .HasMaxLength(25)
            .IsRequired();

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasMaxLength(25)
            .HasConversion(
                x => x.ToString(),
                x => (ApiSettingType)Enum.Parse(typeof(ApiSettingType), x))
            .IsRequired();

        builder.Property(x => x.Value)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasData(
            new ApiSetting(
                "SET_LIMIT_DAY",
                "The maximum number of sets that can be created per day by a user. Zero for unlimited",
                ApiSettingType.Integer,
                Convert.ToString(20)
            )
        );
    }
}