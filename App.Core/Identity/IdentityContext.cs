using App.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace MarminaAttendance.Identity
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        public DbSet<Servants> Servants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
           .Property(e => e.Photo)
           .IsRequired(false);

            // Configure the relationship between ApplicationUser and Servants
            builder.Entity<ApplicationUser>()
                .HasOne(e => e.Servant)
                .WithMany()
                .HasForeignKey(e => e.ServantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Servants entity but ignore all its navigation properties
            // This prevents IdentityContext from trying to load ServantWeek, ServedWeeks, etc.
            builder.Entity<Servants>(entity =>
            {
                entity.ToTable("Servants");
                entity.HasKey(e => e.Id);

                // Ignore all navigation properties that belong to MarminaAttendanceContext
                entity.Ignore(e => e.ServantWeek);
                entity.Ignore(e => e.Served);
                entity.Ignore(e => e.Class);
                entity.Ignore(e => e.Service);
            });

        }
    }


}
