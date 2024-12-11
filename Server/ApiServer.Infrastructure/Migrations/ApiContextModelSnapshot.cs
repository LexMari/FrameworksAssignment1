﻿// <auto-generated />
using System;
using ApiServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiServer.Infrastructure.Migrations
{
    [DbContext(typeof(ApiContext))]
    partial class ApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("ApiServer.Domain.Entities.ApiSetting", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(25)
                        .HasColumnType("TEXT")
                        .HasColumnName("ApiSettingId");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ApiSettings", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "SET_LIMIT_DAY",
                            CreatedAt = new DateTime(2024, 12, 11, 10, 38, 56, 913, DateTimeKind.Local).AddTicks(1199),
                            Description = "The maximum number of sets that can be created per day by a user. Zero for unlimited",
                            Type = "Integer",
                            UpdatedAt = new DateTime(2024, 12, 11, 10, 38, 56, 913, DateTimeKind.Local).AddTicks(1244),
                            Value = "20"
                        });
                });

            modelBuilder.Entity("ApiServer.Domain.Entities.Collection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("CollectionId");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "comment");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Collections", (string)null);
                });

            modelBuilder.Entity("ApiServer.Domain.Entities.CollectionFlashcard", b =>
                {
                    b.Property<int>("CollectionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FlashcardSetId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CollectionId", "FlashcardSetId");

                    b.HasIndex("FlashcardSetId");

                    b.ToTable("CollectionFlashcard");
                });

            modelBuilder.Entity("ApiServer.Domain.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("CommentId");

                    b.Property<int?>("AuthorId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "commentText");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("FlashcardSetId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Rating")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("FlashcardSetId");

                    b.ToTable("Comments", (string)null);
                });

            modelBuilder.Entity("ApiServer.Domain.Entities.FlashCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("FlashCardId");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Difficulty")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FlashcardSetId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FlashcardSetId");

                    b.ToTable("FlashCards", (string)null);
                });

            modelBuilder.Entity("ApiServer.Domain.Entities.FlashcardSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("FlashcardSetId");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Rating")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("FlashcardSets", (string)null);

                    b.HasAnnotation("Relational:JsonPropertyName", "set");
                });

            modelBuilder.Entity("ApiServer.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("UserId");

                    b.Property<bool>("IsAdministrator")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "admin");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("UX_Users_Username");

                    b.ToTable("Users", (string)null);

                    b.HasAnnotation("Relational:JsonPropertyName", "author");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsAdministrator = false,
                            PasswordHash = "keEylKKFJGqonuUMVr9DfsefesIBRBcb+o80PhTQGnq/D0hecj+eUU5/5ATVJPfjvPXW6+fNNB0r/w0Gh1mWSQ==",
                            PasswordSalt = "rozJk1nfLVMg3Ppf2UTcF2WBbwcUDRize72Ap0WawiXsnIjRmODrzX+g4BYdpYuB1dLPIvlA2p8Y0lpuSBJXug==",
                            Username = "student"
                        },
                        new
                        {
                            Id = 2,
                            IsAdministrator = true,
                            PasswordHash = "yWWMDneBej5l9rIt3bIPSTOXZqKyp8bofe4o+yqn+ztp+5gyusoBTjc38OHcAHfej7MeMOqvWqWHp4PvRCCrGw==",
                            PasswordSalt = "kY6VoWsSBJERN2Bt4ej633PpO0Mpq/Hze54WUNSaqYOxoBw4V8LWGG64Rrx6ahDzKGcFtxbEt4b1vlBEoq0ONA==",
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreApplication", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ApplicationType")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientId")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientSecret")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientType")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("ConsentType")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayNames")
                        .HasColumnType("TEXT");

                    b.Property<string>("JsonWebKeySet")
                        .HasColumnType("TEXT");

                    b.Property<string>("Permissions")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostLogoutRedirectUris")
                        .HasColumnType("TEXT");

                    b.Property<string>("Properties")
                        .HasColumnType("TEXT");

                    b.Property<string>("RedirectUris")
                        .HasColumnType("TEXT");

                    b.Property<string>("Requirements")
                        .HasColumnType("TEXT");

                    b.Property<string>("Settings")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("OpenIddictApplications", (string)null);
                });

            modelBuilder.Entity("OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreAuthorization", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ApplicationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Properties")
                        .HasColumnType("TEXT");

                    b.Property<string>("Scopes")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Subject")
                        .HasMaxLength(400)
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId", "Status", "Subject", "Type");

                    b.ToTable("OpenIddictAuthorizations", (string)null);
                });

            modelBuilder.Entity("OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreScope", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Descriptions")
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayNames")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Properties")
                        .HasColumnType("TEXT");

                    b.Property<string>("Resources")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("OpenIddictScopes", (string)null);
                });

            modelBuilder.Entity("OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreToken", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ApplicationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("AuthorizationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Payload")
                        .HasColumnType("TEXT");

                    b.Property<string>("Properties")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("RedemptionDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReferenceId")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Subject")
                        .HasMaxLength(400)
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AuthorizationId");

                    b.HasIndex("ReferenceId")
                        .IsUnique();

                    b.HasIndex("ApplicationId", "Status", "Subject", "Type");

                    b.ToTable("OpenIddictTokens", (string)null);
                });

            modelBuilder.Entity("ApiServer.Domain.Entities.Collection", b =>
                {
                    b.HasOne("ApiServer.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ApiServer.Domain.Entities.CollectionFlashcard", b =>
                {
                    b.HasOne("ApiServer.Domain.Entities.Collection", null)
                        .WithMany()
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiServer.Domain.Entities.FlashcardSet", null)
                        .WithMany()
                        .HasForeignKey("FlashcardSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApiServer.Domain.Entities.Comment", b =>
                {
                    b.HasOne("ApiServer.Domain.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiServer.Domain.Entities.FlashcardSet", "FlashcardSet")
                        .WithMany()
                        .HasForeignKey("FlashcardSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("FlashcardSet");
                });

            modelBuilder.Entity("ApiServer.Domain.Entities.FlashCard", b =>
                {
                    b.HasOne("ApiServer.Domain.Entities.FlashcardSet", "FlashcardSet")
                        .WithMany("Cards")
                        .HasForeignKey("FlashcardSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FlashcardSet");
                });

            modelBuilder.Entity("ApiServer.Domain.Entities.FlashcardSet", b =>
                {
                    b.HasOne("ApiServer.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreAuthorization", b =>
                {
                    b.HasOne("OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreApplication", "Application")
                        .WithMany("Authorizations")
                        .HasForeignKey("ApplicationId");

                    b.Navigation("Application");
                });

            modelBuilder.Entity("OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreToken", b =>
                {
                    b.HasOne("OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreApplication", "Application")
                        .WithMany("Tokens")
                        .HasForeignKey("ApplicationId");

                    b.HasOne("OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreAuthorization", "Authorization")
                        .WithMany("Tokens")
                        .HasForeignKey("AuthorizationId");

                    b.Navigation("Application");

                    b.Navigation("Authorization");
                });

            modelBuilder.Entity("ApiServer.Domain.Entities.FlashcardSet", b =>
                {
                    b.Navigation("Cards");
                });

            modelBuilder.Entity("OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreApplication", b =>
                {
                    b.Navigation("Authorizations");

                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreAuthorization", b =>
                {
                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}
