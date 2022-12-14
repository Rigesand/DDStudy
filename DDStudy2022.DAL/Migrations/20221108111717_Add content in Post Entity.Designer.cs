// <auto-generated />
using System;
using DDStudy2022.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DDStudy2022.DAL.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221108111717_Add content in Post Entity")]
    partial class AddcontentinPostEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DDStudy2022.DAL.Entities.Attach", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Attaches", (string)null);
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserAccountId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserAccountId");

                    b.ToTable("Comments", (string)null);
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserAccountId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserAccountId");

                    b.ToTable("Posts", (string)null);
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserAccountId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.UserAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserAccounts", (string)null);
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.UserSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("RefreshToken")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.Avatar", b =>
                {
                    b.HasBaseType("DDStudy2022.DAL.Entities.Attach");

                    b.Property<Guid>("UserAccountId")
                        .HasColumnType("uuid");

                    b.HasIndex("UserAccountId")
                        .IsUnique();

                    b.ToTable("Avatars", (string)null);
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.Photo", b =>
                {
                    b.HasBaseType("DDStudy2022.DAL.Entities.Attach");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserAccountId")
                        .HasColumnType("uuid");

                    b.HasIndex("PostId");

                    b.HasIndex("UserAccountId");

                    b.ToTable("Photos", (string)null);
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.Comment", b =>
                {
                    b.HasOne("DDStudy2022.DAL.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DDStudy2022.DAL.Entities.UserAccount", "UserAccount")
                        .WithMany("Comments")
                        .HasForeignKey("UserAccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("UserAccount");
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.Post", b =>
                {
                    b.HasOne("DDStudy2022.DAL.Entities.UserAccount", "UserAccount")
                        .WithMany("Posts")
                        .HasForeignKey("UserAccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("UserAccount");
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.UserAccount", b =>
                {
                    b.HasOne("DDStudy2022.DAL.Entities.User", "User")
                        .WithOne("UserAccount")
                        .HasForeignKey("DDStudy2022.DAL.Entities.UserAccount", "UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.UserSession", b =>
                {
                    b.HasOne("DDStudy2022.DAL.Entities.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.Avatar", b =>
                {
                    b.HasOne("DDStudy2022.DAL.Entities.Attach", null)
                        .WithOne()
                        .HasForeignKey("DDStudy2022.DAL.Entities.Avatar", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DDStudy2022.DAL.Entities.UserAccount", "UserAccount")
                        .WithOne("Avatar")
                        .HasForeignKey("DDStudy2022.DAL.Entities.Avatar", "UserAccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("UserAccount");
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.Photo", b =>
                {
                    b.HasOne("DDStudy2022.DAL.Entities.Attach", null)
                        .WithOne()
                        .HasForeignKey("DDStudy2022.DAL.Entities.Photo", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DDStudy2022.DAL.Entities.Post", "Post")
                        .WithMany("Photos")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DDStudy2022.DAL.Entities.UserAccount", "UserAccount")
                        .WithMany()
                        .HasForeignKey("UserAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("UserAccount");
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Photos");
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.User", b =>
                {
                    b.Navigation("Sessions");

                    b.Navigation("UserAccount")
                        .IsRequired();
                });

            modelBuilder.Entity("DDStudy2022.DAL.Entities.UserAccount", b =>
                {
                    b.Navigation("Avatar");

                    b.Navigation("Comments");

                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
