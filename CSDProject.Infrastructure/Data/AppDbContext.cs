using Microsoft.EntityFrameworkCore;
using CSDProject.Domain.Entities;

namespace CSDProject.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<BlacklistedToken> BlacklistedTokens { get; set; }
    public DbSet<CsdStudentContactUs> StudentContactUs { get; set; }
    public DbSet<StudentProjectDetails> StudentProjectDetails { get; set; }
    public DbSet<Notice> Notices { get; set; }
    public DbSet<Announcement> Announcements { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("csd_user_registration");
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        
        // Configure BlacklistedToken table name to match your database
        modelBuilder.Entity<BlacklistedToken>().ToTable("blacklisted_tokens");

        // Configure Notice entity
        modelBuilder.Entity<Notice>()
            .ToTable("csd_notices")
            .HasKey(n => n.NoticeId);
        
        modelBuilder.Entity<Notice>()
            .HasOne(n => n.Creator)
            .WithMany()
            .HasForeignKey(n => n.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Announcement entity
        modelBuilder.Entity<Announcement>()
            .ToTable("csd_announcements")
            .HasKey(a => a.AnnouncementId);
        
        modelBuilder.Entity<Announcement>()
            .HasOne(a => a.Creator)
            .WithMany()
            .HasForeignKey(a => a.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
