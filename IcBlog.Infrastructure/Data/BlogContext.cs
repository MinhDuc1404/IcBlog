using IcBlog.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IcBlog.Infrastructure.Data
{
    public class BlogContext : IdentityDbContext<ApplicationUser>
    {
        public BlogContext(DbContextOptions<BlogContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed roles
            var admin = new IdentityRole("admin");
            admin.NormalizedName = "admin";

            var user = new IdentityRole("user");
            user.NormalizedName = "user";

            builder.Entity<IdentityRole>().HasData(admin, user);

            // Configure Comment relationships
            builder.Entity<Comment>()
    .HasOne(c => c.Blog)
    .WithMany(b => b.Comments)
    .HasForeignKey(c => c.BlogID)
    .OnDelete(DeleteBehavior.Cascade); // Enable cascade delete

            builder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorID)
                .OnDelete(DeleteBehavior.SetNull); // Set AuthorID to null when User is deleted

            builder.Entity<Comment>()
                .HasOne(c => c.CommentParent)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.CommentParentID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete for nested comments

        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
    }
}
    