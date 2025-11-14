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
        // Configure User entity - Table already exists from Java project
        modelBuilder.Entity<User>()
            .ToTable("csd_user_registration", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        
        // Configure BlacklistedToken - Table already exists from Java project
        modelBuilder.Entity<BlacklistedToken>()
            .ToTable("blacklisted_tokens", t => t.ExcludeFromMigrations());

        // Configure StudentContactUs - NEW table to be created
        modelBuilder.Entity<CsdStudentContactUs>()
            .ToTable("csd_Student_ContactUS")
            .HasKey(c => c.ContactId);

        // Configure StudentProjectDetails - NEW table to be created
        modelBuilder.Entity<StudentProjectDetails>()
            .ToTable("csd_Student_ProjectDetails")
            .HasKey(p => p.ProjectId);

        // Configure Notice entity - NEW table to be created
        modelBuilder.Entity<Notice>()
            .ToTable("csd_notices")
            .HasKey(n => n.NoticeId);
        
        modelBuilder.Entity<Notice>()
            .HasOne(n => n.Creator)
            .WithMany()
            .HasForeignKey(n => n.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Announcement entity - NEW table to be created
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
