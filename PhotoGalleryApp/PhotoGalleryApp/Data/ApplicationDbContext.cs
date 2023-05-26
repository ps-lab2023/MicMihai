using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhotoGalleryApp.Models;

namespace PhotoGalleryApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<TagPhoto> TagPhotos { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { 

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User-Album relationship (1:n)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Albums)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            // Configure Album-Photo relationship (1:n)
            modelBuilder.Entity<Album>()
                .HasMany(a => a.Photos)
                .WithOne(p => p.Album)
                .HasForeignKey(p => p.AlbumId);

            // Configure Photo-Tag relationship (n:n)
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

            // Configure User-Comment relationship (1:n)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            // Configure Photo-Comment relationship (1:n)
            modelBuilder.Entity<Photo>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Photo)
                .HasForeignKey(c => c.PhotoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Photo)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PhotoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
    