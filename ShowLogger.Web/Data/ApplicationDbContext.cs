using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ShowLogger.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.HasSequence<int>("OA_IDENTITY_USER_ID_SEQ")
                .StartsAt(1000)
                .IncrementsBy(1);

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(m => m.UserId)
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .IsRequired()
                .HasDefaultValueSql("NEXT VALUE FOR OA_IDENTITY_USER_ID_SEQ");

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(250);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(250);

        }
    }
}