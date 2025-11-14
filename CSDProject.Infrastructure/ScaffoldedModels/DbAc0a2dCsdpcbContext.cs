using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class DbAc0a2dCsdpcbContext : DbContext
{
    public DbAc0a2dCsdpcbContext()
    {
    }

    public DbAc0a2dCsdpcbContext(DbContextOptions<DbAc0a2dCsdpcbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlacklistedToken> BlacklistedTokens { get; set; }

    public virtual DbSet<CsdAdminRegistration> CsdAdminRegistrations { get; set; }

    public virtual DbSet<CsdCourseDetail> CsdCourseDetails { get; set; }

    public virtual DbSet<CsdEmailValidation> CsdEmailValidations { get; set; }

    public virtual DbSet<CsdFacultyExperience> CsdFacultyExperiences { get; set; }

    public virtual DbSet<CsdFacultyRegistration> CsdFacultyRegistrations { get; set; }

    public virtual DbSet<CsdStudentRegistration> CsdStudentRegistrations { get; set; }

    public virtual DbSet<CsdSubjectDetail> CsdSubjectDetails { get; set; }

    public virtual DbSet<CsdUserRegistration> CsdUserRegistrations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=SQL1003.site4now.net;Initial Catalog=db_ac0a2d_csdpcb;User Id=db_ac0a2d_csdpcb_admin;Password=bcpm@100");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlacklistedToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__blacklis__3213E83FF323E102");

            entity.ToTable("blacklisted_tokens");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.Token)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("token");
        });

        modelBuilder.Entity<CsdAdminRegistration>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__csd_admi__43AA4141267F1734");

            entity.ToTable("csd_admin_registration");

            entity.HasIndex(e => e.UserId, "UKe0e3w8oquabi18j887vq4yfb2")
                .IsUnique()
                .HasFilter("([user_id] IS NOT NULL)");

            entity.Property(e => e.AdminId)
                .ValueGeneratedNever()
                .HasColumnName("admin_id");
            entity.Property(e => e.ApproveStatus)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("approve_status");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedOn).HasColumnName("created_on");
            entity.Property(e => e.DateOfJoining).HasColumnName("date_of_joining");
            entity.Property(e => e.Designation)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("designation");
            entity.Property(e => e.Qualification)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("qualification");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.CsdAdminRegistration)
                .HasForeignKey<CsdAdminRegistration>(d => d.UserId)
                .HasConstraintName("FK7gnws3a1f1o36solb39vqf96e");
        });

        modelBuilder.Entity<CsdCourseDetail>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__csd_cour__8F1EF7AEE1948DD7");

            entity.ToTable("csd_course_details");

            entity.Property(e => e.CourseId)
                .ValueGeneratedNever()
                .HasColumnName("course_id");
            entity.Property(e => e.CourseName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("course_name");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Level)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("level");
        });

        modelBuilder.Entity<CsdEmailValidation>(entity =>
        {
            entity.HasKey(e => e.EmailId).HasName("PK__csd_emai__3FEF876680EC585B");

            entity.ToTable("csd_email_validation");

            entity.Property(e => e.EmailId)
                .ValueGeneratedNever()
                .HasColumnName("email_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.ExpiryTime)
                .HasPrecision(6)
                .HasColumnName("expiry_time");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Otp).HasColumnName("otp");
            entity.Property(e => e.OtpStatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("otp_status");
        });

        modelBuilder.Entity<CsdFacultyExperience>(entity =>
        {
            entity.HasKey(e => e.ExperienceId).HasName("PK__csd_facu__EB216AFC17A03A91");

            entity.ToTable("csd_faculty_experience");

            entity.Property(e => e.ExperienceId)
                .ValueGeneratedNever()
                .HasColumnName("experience_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.FacultyId).HasColumnName("faculty_id");
            entity.Property(e => e.InstituteName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("institute_name");
            entity.Property(e => e.Position)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("position");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Faculty).WithMany(p => p.CsdFacultyExperiences)
                .HasForeignKey(d => d.FacultyId)
                .HasConstraintName("FKcdkbul2s9maohwegdq55fl0op");
        });

        modelBuilder.Entity<CsdFacultyRegistration>(entity =>
        {
            entity.HasKey(e => e.FacultyId).HasName("PK__csd_facu__7B00413CD25BD6B3");

            entity.ToTable("csd_faculty_registration");

            entity.HasIndex(e => e.UserId, "UKjm7at0xil467qooptpjds57mp")
                .IsUnique()
                .HasFilter("([user_id] IS NOT NULL)");

            entity.Property(e => e.FacultyId)
                .ValueGeneratedNever()
                .HasColumnName("faculty_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedOn).HasColumnName("created_on");
            entity.Property(e => e.DateOfJoining).HasColumnName("date_of_joining");
            entity.Property(e => e.Designation)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("designation");
            entity.Property(e => e.Qualification)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("qualification");
            entity.Property(e => e.Specialisation)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("specialisation");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.CsdFacultyRegistration)
                .HasForeignKey<CsdFacultyRegistration>(d => d.UserId)
                .HasConstraintName("FK17wbmvke4cjgcyrye6cqwxywe");
        });

        modelBuilder.Entity<CsdStudentRegistration>(entity =>
        {
            entity.HasKey(e => e.StdId).HasName("PK__csd_stud__0B0245BAAE0FDCC7");

            entity.ToTable("csd_student_registration");

            entity.HasIndex(e => e.UserId, "UKc46rmmf11mthqf2349ik9h2vm")
                .IsUnique()
                .HasFilter("([user_id] IS NOT NULL)");

            entity.HasIndex(e => e.RollNumber, "UKhkvplqnyv2dbpc47k4w3kwtuk")
                .IsUnique()
                .HasFilter("([roll_number] IS NOT NULL)");

            entity.HasIndex(e => e.UnivRollNumber, "UKoap7xyg76xmmyh4q7e4du90g5")
                .IsUnique()
                .HasFilter("([univ_roll_number] IS NOT NULL)");

            entity.Property(e => e.StdId)
                .ValueGeneratedNever()
                .HasColumnName("std_id");
            entity.Property(e => e.AcademicYear).HasColumnName("academic_year");
            entity.Property(e => e.AdmissionDate).HasColumnName("admission_date");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedOn).HasColumnName("created_on");
            entity.Property(e => e.CurrentSemester).HasColumnName("current_semester");
            entity.Property(e => e.RollNumber).HasColumnName("roll_number");
            entity.Property(e => e.UnivRollNumber).HasColumnName("univ_roll_number");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Course).WithMany(p => p.CsdStudentRegistrations)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKcngox7itxh0qq4v2l85t4ro7u");

            entity.HasOne(d => d.User).WithOne(p => p.CsdStudentRegistration)
                .HasForeignKey<CsdStudentRegistration>(d => d.UserId)
                .HasConstraintName("FKtaeregqrsxulfnpp6vw3us0j8");
        });

        modelBuilder.Entity<CsdSubjectDetail>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__csd_subj__5004F6601FF7D738");

            entity.ToTable("csd_subject_details");

            entity.Property(e => e.SubjectId)
                .ValueGeneratedNever()
                .HasColumnName("subject_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.Semester).HasColumnName("semester");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("subject_name");
            entity.Property(e => e.SyllabusUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("syllabus_url");

            entity.HasOne(d => d.Course).WithMany(p => p.CsdSubjectDetails)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FKo4y33f4svsjr50k16l8mtg3dq");
        });

        modelBuilder.Entity<CsdUserRegistration>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__csd_user__B9BE370F911A3DB8");

            entity.ToTable("csd_user_registration");

            entity.HasIndex(e => e.Email, "UK83snerxhjgwqcfv2eoeglrjoa")
                .IsUnique()
                .HasFilter("([email] IS NOT NULL)");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.AccountStatus)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("account_status");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedOn).HasColumnName("created_on");
            entity.Property(e => e.DeviceId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("device_id");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.MobileNumber).HasColumnName("mobile_number");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.ProfilePhotoUrl)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("profile_photo_url");
            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
        });
        modelBuilder.HasSequence("csd_admin_registration_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_course_details_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_email_validation_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_subject_details_seq").IncrementsBy(50);
        modelBuilder.HasSequence("seq1").StartsAt(1001L);
        modelBuilder.HasSequence("seq2").StartsAt(10001L);
        modelBuilder.HasSequence("seq3").StartsAt(100L);
        modelBuilder.HasSequence("seq4").StartsAt(50L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
