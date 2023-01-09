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
    [Migration("20230108175236_0.1.0007")]
    partial class _010007
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

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

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_SHOW", b =>
                {
                    b.Property<int>("SHOW_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DATE_WATCHED")
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
#pragma warning restore 612, 618
        }
    }
}
