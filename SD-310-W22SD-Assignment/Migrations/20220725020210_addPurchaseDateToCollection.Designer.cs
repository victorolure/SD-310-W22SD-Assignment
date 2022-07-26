﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SD_310_W22SD_Assignment.Models;

#nullable disable

namespace SD_310_W22SD_Assignment.Migrations
{
    [DbContext(typeof(MyTunesContext))]
    [Migration("20220725020210_addPurchaseDateToCollection")]
    partial class addPurchaseDateToCollection
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SD_310_W22SD_Assignment.Models.Artist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nchar(50)")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.ToTable("Artist", (string)null);
                });

            modelBuilder.Entity("SD_310_W22SD_Assignment.Models.Collection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SongId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SongId");

                    b.HasIndex("UserId");

                    b.ToTable("Collection", (string)null);
                });

            modelBuilder.Entity("SD_310_W22SD_Assignment.Models.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Artist")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nchar(50)")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.HasIndex("Artist");

                    b.ToTable("Song", (string)null);
                });

            modelBuilder.Entity("SD_310_W22SD_Assignment.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Wallet")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("SD_310_W22SD_Assignment.Models.Collection", b =>
                {
                    b.HasOne("SD_310_W22SD_Assignment.Models.Song", "Song")
                        .WithMany("Collections")
                        .HasForeignKey("SongId")
                        .IsRequired()
                        .HasConstraintName("FK_Collection_Song");

                    b.HasOne("SD_310_W22SD_Assignment.Models.User", "User")
                        .WithMany("Collections")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Collection_User");

                    b.Navigation("Song");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SD_310_W22SD_Assignment.Models.Song", b =>
                {
                    b.HasOne("SD_310_W22SD_Assignment.Models.Artist", "ArtistNavigation")
                        .WithMany("Songs")
                        .HasForeignKey("Artist")
                        .IsRequired()
                        .HasConstraintName("FK_Song_Artist");

                    b.Navigation("ArtistNavigation");
                });

            modelBuilder.Entity("SD_310_W22SD_Assignment.Models.Artist", b =>
                {
                    b.Navigation("Songs");
                });

            modelBuilder.Entity("SD_310_W22SD_Assignment.Models.Song", b =>
                {
                    b.Navigation("Collections");
                });

            modelBuilder.Entity("SD_310_W22SD_Assignment.Models.User", b =>
                {
                    b.Navigation("Collections");
                });
#pragma warning restore 612, 618
        }
    }
}
