using Microsoft.EntityFrameworkCore;
using StudentsApi.Domain.Entities;

namespace StudentsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        {
        }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<PhotoAvatar> PhotoAvatars => Set<PhotoAvatar>();
        public DbSet<PhotoEnclosure> PhotoEnclosures => Set<PhotoEnclosure>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(x => x.LastName).HasMaxLength(100).IsRequired();
                entity.Property(x => x.Email).HasMaxLength(255).IsRequired();
                entity.Property(x => x.Notes).HasMaxLength(4000);
                entity.Property(x => x.CreatedAt).IsRequired();

                // unique
                entity.HasIndex(x => x.Email).IsUnique();

                // 1-1 Student -> PhotoAvatar
                entity.HasOne(s => s.Avatar)
                    .WithOne(a => a.Student)
                    .HasForeignKey<PhotoAvatar>(a => a.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                // 1-many Student -> PhotoEnclosure
                entity.HasMany(s => s.Enclosures)
                    .WithOne(e => e.Student)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PhotoAvatar>(entity =>
            {
                entity.Property(x => x.FileName).HasMaxLength(255).IsRequired();
                entity.Property(x => x.StoredFileName).HasMaxLength(255).IsRequired();
                entity.Property(x => x.ContentType).HasMaxLength(100).IsRequired();
                entity.Property(x => x.Size).IsRequired();
                entity.Property(x => x.UploadedAt).IsRequired();

                // unique
                entity.HasIndex(x => x.StudentId).IsUnique();
                entity.HasIndex(x => x.StoredFileName).IsUnique();
            });

            modelBuilder.Entity<PhotoEnclosure>(entity =>
            {
                entity.Property(x => x.FileName).HasMaxLength(255).IsRequired();
                entity.Property(x => x.StoredFileName).HasMaxLength(255).IsRequired();
                entity.Property(x => x.ContentType).HasMaxLength(100).IsRequired();
                entity.Property(x => x.Size).IsRequired();
                entity.Property(x => x.UploadedAt).IsRequired();

                // unique
                entity.HasIndex(x => x.StudentId);
                entity.HasIndex(x => x.StoredFileName).IsUnique();
            });
        }
    }
}
