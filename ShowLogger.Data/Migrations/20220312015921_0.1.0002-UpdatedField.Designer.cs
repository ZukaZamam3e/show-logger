﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShowLogger.Data.Context;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    [DbContext(typeof(ShowLoggerDbContext))]
    [Migration("20220312015921_0.1.0002-UpdatedField")]
    partial class _010002UpdatedField
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.HasSequence<int>("SL_SHOW_SHOW_ID_SEQ")
                .StartsAt(1000L);

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_CODE_VALUE", b =>
                {
                    b.Property<int>("CODE_VALUE_ID")
                        .HasColumnType("int");

                    b.Property<int>("CODE_TABLE_ID")
                        .HasColumnType("int");

                    b.Property<string>("DECODE_TXT")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EXTRA_INFO")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

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
                        });
                });

            modelBuilder.Entity("ShowLogger.Data.Entities.SL_SHOW", b =>
                {
                    b.Property<int>("SHOW_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR SL_SHOW_SHOW_ID_SEQ");

                    b.Property<DateTime>("DATE_WATCHED")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EPISODE_NUMBER")
                        .HasColumnType("int");

                    b.Property<int?>("SEASON_NUMBER")
                        .HasColumnType("int");

                    b.Property<string>("SHOW_NAME")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SHOW_TYPE_ID")
                        .HasColumnType("int");

                    b.Property<int>("USER_ID")
                        .HasColumnType("int");

                    b.HasKey("SHOW_ID");

                    b.ToTable("SL_SHOW");
                });
#pragma warning restore 612, 618
        }
    }
}
