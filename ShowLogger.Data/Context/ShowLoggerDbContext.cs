using Microsoft.EntityFrameworkCore;
using ShowLogger.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Data.Context;

public class ShowLoggerDbContext : DbContext
{
    public ShowLoggerDbContext(DbContextOptions<ShowLoggerDbContext> options)
        : base(options)
    {
        int x = 0;
    }

    public DbSet<SL_SHOW> SL_SHOW { get; set; }
    public DbSet<SL_CODE_VALUE> SL_CODE_VALUE { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SL_SHOW>().HasKey(m => m.SHOW_ID);
        modelBuilder.Entity<SL_CODE_VALUE>().HasKey(m => m.CODE_VALUE_ID);

        modelBuilder.Entity<SL_CODE_VALUE>(entity =>
        {
            entity.Property(e => e.CODE_TABLE_ID)
                .ValueGeneratedNever();

            entity.Property(e => e.CODE_VALUE_ID)
                .ValueGeneratedNever();

            entity.Property(e => e.DECODE_TXT)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.EXTRA_INFO)
                .HasMaxLength(100);
        });

        modelBuilder.Entity<SL_CODE_VALUE>().HasData(
            new SL_CODE_VALUE { CODE_TABLE_ID = (int)CodeTableIds.SHOW_TYPE_ID, CODE_VALUE_ID = (int)CodeValueIds.TV, DECODE_TXT = "TV" },
            new SL_CODE_VALUE { CODE_TABLE_ID = (int)CodeTableIds.SHOW_TYPE_ID, CODE_VALUE_ID = (int)CodeValueIds.MOVIE, DECODE_TXT = "Movie" },
            new SL_CODE_VALUE { CODE_TABLE_ID = (int)CodeTableIds.SHOW_TYPE_ID, CODE_VALUE_ID = (int)CodeValueIds.AMC, DECODE_TXT = "AMC" }
        );

        modelBuilder.HasSequence<int>("SL_SHOW_SHOW_ID_SEQ")
            .StartsAt(1000)
            .IncrementsBy(1);

        modelBuilder.Entity<SL_SHOW>(entity =>
        {
            entity.Property(e => e.SHOW_ID)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR SL_SHOW_SHOW_ID_SEQ");

            entity.Property(e => e.SHOW_NAME)
                .IsRequired()
                .HasMaxLength(100);
        });

    }
}
