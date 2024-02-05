﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShowLogger.Data.Context;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    [DbContext(typeof(ShowLoggerDbContext))]
    [Migration("20240204171515_0.1.0019")]
    partial class _010019
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ShowLogger.Data.Entities.OA_USERS", b =>
                {
                    b.Property<int>("USER_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FIRST_NAME")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LAST_NAME")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("USER_NAME")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("USER_ID");

                    b.ToTable("OA_USERS");
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_BOOK", b =>
                {
                    b.Property<int>("BOOK_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BOOK_NAME")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("BOOK_NOTES")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<int?>("CHAPTERS")
                        .HasColumnType("int");

                    b.Property<DateTime?>("END_DATE")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("PAGES")
                        .HasColumnType("int");

                    b.Property<DateTime?>("START_DATE")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("USER_ID")
                        .HasColumnType("int");

                    b.HasKey("BOOK_ID");

                    b.ToTable("SL_BOOK");
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_CODE_VALUE", b =>
                {
                    b.Property<int>("CODE_VALUE_ID")
                        .HasColumnType("int");

                    b.Property<int>("CODE_TABLE_ID")
                        .HasColumnType("int");

                    b.Property<string>("DECODE_TXT")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("EXTRA_INFO")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("CODE_VALUE_ID");

                    b.ToTable("SL_CODE_VALUE");

                    b.HasData(
                        new
                        {
                            CODE_VALUE_ID = 1000,
                            CODE_TABLE_ID = 1,
                            DECODE_TXT = "TV"
                        },
                        new
                        {
                            CODE_VALUE_ID = 1001,
                            CODE_TABLE_ID = 1,
                            DECODE_TXT = "Movie"
                        },
                        new
                        {
                            CODE_VALUE_ID = 1002,
                            CODE_TABLE_ID = 1,
                            DECODE_TXT = "AMC"
                        },
                        new
                        {
                            CODE_VALUE_ID = 2000,
                            CODE_TABLE_ID = 2,
                            DECODE_TXT = "A-list Ticket"
                        },
                        new
                        {
                            CODE_VALUE_ID = 2001,
                            CODE_TABLE_ID = 2,
                            DECODE_TXT = "Ticket"
                        },
                        new
                        {
                            CODE_VALUE_ID = 2002,
                            CODE_TABLE_ID = 2,
                            DECODE_TXT = "Purchase"
                        },
                        new
                        {
                            CODE_VALUE_ID = 2003,
                            CODE_TABLE_ID = 2,
                            DECODE_TXT = "AMC A-list"
                        });
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_FRIEND", b =>
                {
                    b.Property<int>("FRIEND_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CREATED_DATE")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FRIEND_USER_ID")
                        .HasColumnType("int");

                    b.Property<int>("USER_ID")
                        .HasColumnType("int");

                    b.HasKey("FRIEND_ID");

                    b.ToTable("SL_FRIEND");
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_FRIEND_REQUEST", b =>
                {
                    b.Property<int>("FRIEND_REQUEST_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DATE_SENT")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("RECEIVED_USER_ID")
                        .HasColumnType("int");

                    b.Property<int>("SENT_USER_ID")
                        .HasColumnType("int");

                    b.HasKey("FRIEND_REQUEST_ID");

                    b.ToTable("SL_FRIEND_REQUEST");
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_MOVIE_INFO", b =>
                {
                    b.Property<int>("MOVIE_INFO_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("AIR_DATE")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("IMAGE_URL")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LAST_DATA_REFRESH")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LAST_UPDATED")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MOVIE_NAME")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("MOVIE_OVERVIEW")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("varchar(4000)");

                    b.Property<string>("OMDB_ID")
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<string>("OTHER_NAMES")
                        .HasColumnType("longtext");

                    b.Property<int?>("RUNTIME")
                        .HasColumnType("int");

                    b.Property<int?>("TMDB_ID")
                        .HasColumnType("int");

                    b.HasKey("MOVIE_INFO_ID");

                    b.ToTable("SL_MOVIE_INFO");
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_SHOW", b =>
                {
                    b.Property<int>("SHOW_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DATE_WATCHED")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("EPISODE_NUMBER")
                        .HasColumnType("int");

                    b.Property<int?>("INFO_ID")
                        .HasColumnType("int");

                    b.Property<bool>("RESTART_BINGE")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("SEASON_NUMBER")
                        .HasColumnType("int");

                    b.Property<string>("SHOW_NAME")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("SHOW_NOTES")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<int>("SHOW_TYPE_ID")
                        .HasColumnType("int");

                    b.Property<int>("USER_ID")
                        .HasColumnType("int");

                    b.HasKey("SHOW_ID");

                    b.ToTable("SL_SHOW");
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_TRANSACTION", b =>
                {
                    b.Property<int>("TRANSACTION_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal?>("BENEFIT_AMT")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("COST_AMT")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal?>("DISCOUNT_AMT")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("ITEM")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("SHOW_ID")
                        .HasColumnType("int");

                    b.Property<DateTime>("TRANSACTION_DATE")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TRANSACTION_NOTES")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<int>("TRANSACTION_TYPE_ID")
                        .HasColumnType("int");

                    b.Property<int>("USER_ID")
                        .HasColumnType("int");

                    b.HasKey("TRANSACTION_ID");

                    b.ToTable("SL_TRANSACTION");
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_TV_EPISODE_INFO", b =>
                {
                    b.Property<int>("TV_EPISODE_INFO_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("AIR_DATE")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EPISODE_NAME")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<int?>("EPISODE_NUMBER")
                        .HasColumnType("int");

                    b.Property<string>("EPISODE_OVERVIEW")
                        .HasMaxLength(4000)
                        .HasColumnType("varchar(4000)");

                    b.Property<string>("IMAGE_URL")
                        .HasColumnType("longtext");

                    b.Property<string>("OMDB_ID")
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<int?>("RUNTIME")
                        .HasColumnType("int");

                    b.Property<string>("SEASON_NAME")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("SEASON_NUMBER")
                        .HasColumnType("int");

                    b.Property<int?>("TMDB_ID")
                        .HasColumnType("int");

                    b.Property<int>("TV_INFO_ID")
                        .HasColumnType("int");

                    b.HasKey("TV_EPISODE_INFO_ID");

                    b.HasIndex("TV_INFO_ID");

                    b.ToTable("SL_TV_EPISODE_INFO");
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_TV_INFO", b =>
                {
                    b.Property<int>("TV_INFO_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("IMAGE_URL")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LAST_DATA_REFRESH")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LAST_UPDATED")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("OMDB_ID")
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<string>("OTHER_NAMES")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("SHOW_NAME")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("SHOW_OVERVIEW")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("varchar(4000)");

                    b.Property<int?>("TMDB_ID")
                        .HasColumnType("int");

                    b.HasKey("TV_INFO_ID");

                    b.ToTable("SL_TV_INFO");
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_USER_PREF", b =>
                {
                    b.Property<int>("USER_PREF_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DEFAULT_AREA")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("USER_ID")
                        .HasColumnType("int");

                    b.HasKey("USER_PREF_ID");

                    b.ToTable("SL_USER_PREF");
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_WATCHLIST", b =>
                {
                    b.Property<int>("WATCHLIST_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DATE_ADDED")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("EPISODE_NUMBER")
                        .HasColumnType("int");

                    b.Property<int?>("SEASON_NUMBER")
                        .HasColumnType("int");

                    b.Property<string>("SHOW_NAME")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("SHOW_NOTES")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<int>("SHOW_TYPE_ID")
                        .HasColumnType("int");

                    b.Property<int>("USER_ID")
                        .HasColumnType("int");

                    b.HasKey("WATCHLIST_ID");

                    b.ToTable("SL_WATCHLIST");
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_TV_EPISODE_INFO", b =>
                {
                    b.HasOne("ShowLogger.Data.Entities.SL_TV_INFO", "TV_INFO")
                        .WithMany("EPISODE_INFOS")
                        .HasForeignKey("TV_INFO_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TV_INFO");
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_TV_INFO", b =>
                {
                    b.Navigation("EPISODE_INFOS");
                });
#pragma warning restore 612, 618
        }
    }
}
