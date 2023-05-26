using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<TagPhoto> TagPhotos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TagPhoto>()
                .HasKey(tp => new { tp.TagId, tp.PhotoId });

            modelBuilder.Entity<TagPhoto>()
                .HasOne(tp => tp.Tag)
                .WithMany(t => t.TagPhotos)
                .HasForeignKey(tp => tp.TagId);

            modelBuilder.Entity<TagPhoto>()
                .HasOne(tp => tp.Photo)
                .WithMany(p => p.TagPhotos)
                .HasForeignKey(tp => tp.PhotoId);
        }
    }
}