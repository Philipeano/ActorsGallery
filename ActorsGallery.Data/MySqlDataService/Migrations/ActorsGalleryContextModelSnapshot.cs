﻿// <auto-generated />
using System;
using ActorsGallery.Data.MySqlDataService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ActorsGallery.Data.MySqlDataService.Migrations
{
    [DbContext(typeof(ActorsGalleryContext))]
    partial class ActorsGalleryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ActorsGallery.Core.Models.Character", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("LocationId")
                        .HasColumnType("bigint");

                    b.Property<string>("StateOfOrigin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("ActorsGallery.Core.Models.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CommentId")
                        .HasColumnType("bigint");

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasColumnName("Comment")
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("CommenterName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<long>("EpisodeId")
                        .HasColumnType("bigint");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnName("IpAddressLocation")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EpisodeId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ActorsGallery.Core.Models.Episode", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EpisodeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<string>("EpisodeCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Episodes");
                });

            modelBuilder.Entity("ActorsGallery.Core.Models.EpisodeCharacter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<long>("EpisodeId")
                        .HasColumnType("bigint");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("EpisodeId");

                    b.ToTable("EpisodeCharacters");
                });

            modelBuilder.Entity("ActorsGallery.Core.Models.Location", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("LocationId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<double>("Latitude")
                        .HasColumnType("double");

                    b.Property<double>("Longitude")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("ActorsGallery.Core.Models.Character", b =>
                {
                    b.HasOne("ActorsGallery.Core.Models.Location", "Location")
                        .WithMany("Characters")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ActorsGallery.Core.Models.Comment", b =>
                {
                    b.HasOne("ActorsGallery.Core.Models.Episode", "Episode")
                        .WithMany("EpisodeComments")
                        .HasForeignKey("EpisodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ActorsGallery.Core.Models.EpisodeCharacter", b =>
                {
                    b.HasOne("ActorsGallery.Core.Models.Character", "Character")
                        .WithMany("EpisodesFeaturedIn")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ActorsGallery.Core.Models.Episode", "Episode")
                        .WithMany("EpisodeCharacters")
                        .HasForeignKey("EpisodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
