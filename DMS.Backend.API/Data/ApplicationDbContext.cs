using DMS.Backend.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DMS.Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        { }

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentShare> DocumentShares { get; set; }
        public DbSet<DocumentTag> DocumentTags { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ExternalStorage> ExternalStorages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1) Let IdentityDbContext set up the Identity tables, keys, relationships
            base.OnModelCreating(modelBuilder);

            // 2) Now apply your custom configurations:

            modelBuilder.Entity<Notification>()
                .Ignore(n => n.UpdatedDate)
                .Ignore(n => n.IsUpdated);

            modelBuilder.Entity<Document>()
                .Property(d => d.UpdatedDate)
                .IsRequired();

            modelBuilder.Entity<Document>()
                .Property(d => d.IsUpdated)
                .IsRequired();

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.FriendUser)
                .WithMany()
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Sender)
                .WithMany()
                .HasForeignKey(fr => fr.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany()
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}


