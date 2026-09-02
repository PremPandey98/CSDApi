using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class DbAbe381CsddbContext : DbContext
{
    public DbAbe381CsddbContext()
    {
    }

    public DbAbe381CsddbContext(DbContextOptions<DbAbe381CsddbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlacklistedToken> BlacklistedTokens { get; set; }

    public virtual DbSet<CourseMode> CourseModes { get; set; }

    public virtual DbSet<CsdAdminRegistration> CsdAdminRegistrations { get; set; }

    public virtual DbSet<CsdAlumniDetail> CsdAlumniDetails { get; set; }

    public virtual DbSet<CsdAmdinNotice> CsdAmdinNotices { get; set; }

    public virtual DbSet<CsdCourseDetail> CsdCourseDetails { get; set; }

    public virtual DbSet<CsdEmailValidation> CsdEmailValidations { get; set; }

    public virtual DbSet<CsdEventGallery> CsdEventGalleries { get; set; }

    public virtual DbSet<CsdEventPhoto> CsdEventPhotos { get; set; }

    public virtual DbSet<CsdFacultyExperience> CsdFacultyExperiences { get; set; }

    public virtual DbSet<CsdFacultyRegistration> CsdFacultyRegistrations { get; set; }

    public virtual DbSet<CsdStudentAttendance> CsdStudentAttendances { get; set; }

    public virtual DbSet<CsdStudentChallengeResponse> CsdStudentChallengeResponses { get; set; }

    public virtual DbSet<CsdStudentRegistration> CsdStudentRegistrations { get; set; }

    public virtual DbSet<CsdSubjectDetail> CsdSubjectDetails { get; set; }

    public virtual DbSet<CsdTeacgerResource> CsdTeacgerResources { get; set; }

    public virtual DbSet<CsdTeacherAttendanceQr> CsdTeacherAttendanceQrs { get; set; }

    public virtual DbSet<CsdTeacherChallengeDetail> CsdTeacherChallengeDetails { get; set; }

    public virtual DbSet<CsdTeacherChallengePoint> CsdTeacherChallengePoints { get; set; }

    public virtual DbSet<CsdTeacherChallengeQuestion> CsdTeacherChallengeQuestions { get; set; }

    public virtual DbSet<CsdTeacherNoticesDetail> CsdTeacherNoticesDetails { get; set; }

    public virtual DbSet<CsdTeacherTimetableDetail> CsdTeacherTimetableDetails { get; set; }

    public virtual DbSet<CsdTeacherTimetableEntry> CsdTeacherTimetableEntries { get; set; }

    public virtual DbSet<CsdUserRegistration> CsdUserRegistrations { get; set; }

    public virtual DbSet<FoCourseDetail> FoCourseDetails { get; set; }

    public virtual DbSet<FoEnquiryDetail> FoEnquiryDetails { get; set; }

    public virtual DbSet<FoEnquiryStatus> FoEnquiryStatuses { get; set; }

    public virtual DbSet<FoUserAccountDetail> FoUserAccountDetails { get; set; }

    public virtual DbSet<FopCourseDetail> FopCourseDetails { get; set; }

    public virtual DbSet<FopEnquiryDetail> FopEnquiryDetails { get; set; }

    public virtual DbSet<FopEnquiryStatus> FopEnquiryStatuses { get; set; }

    public virtual DbSet<FopUserAccountDetail> FopUserAccountDetails { get; set; }

    public virtual DbSet<InternalMarkDetailsEntity> InternalMarkDetailsEntities { get; set; }

    public virtual DbSet<MailDetail> MailDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=SQL8005.site4now.net;Database=db_ac44d9_csddb;User Id=db_ac44d9_csddb_admin;Password=bcpm@100;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlacklistedToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__blacklis__3213E83F256C8FC1");

            entity.ToTable("blacklisted_tokens");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.Token)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("token");
        });

        modelBuilder.Entity<CourseMode>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("course_modes");

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.Mode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("mode");

            entity.HasOne(d => d.Course).WithMany()
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK7ttn5u2pmwdi2n6flpsoshcnv");
        });

        modelBuilder.Entity<CsdAdminRegistration>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__csd_admi__43AA4141960C1657");

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

        modelBuilder.Entity<CsdAlumniDetail>(entity =>
        {
            entity.HasKey(e => e.AluminiId).HasName("PK__csd_alum__BEB4F6EBFE4AAF78");

            entity.ToTable("csd_alumni_details");

            entity.Property(e => e.AluminiId)
                .ValueGeneratedNever()
                .HasColumnName("alumini_id");
            entity.Property(e => e.Designation)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("designation");
            entity.Property(e => e.Email)
                .HasMaxLength(55)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.LinkedInUrl)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("linked_in_url");
            entity.Property(e => e.Name)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Oraganization)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("oraganization");
            entity.Property(e => e.PassoutYear).HasColumnName("passout_year");
            entity.Property(e => e.Specialization)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("specialization");
        });

        modelBuilder.Entity<CsdAmdinNotice>(entity =>
        {
            entity.HasKey(e => e.NoticeId).HasName("PK__csd_amdi__3E82A5DB5A8DEFDD");

            entity.ToTable("csd_amdin_notices");

            entity.Property(e => e.NoticeId)
                .ValueGeneratedNever()
                .HasColumnName("notice_id");
            entity.Property(e => e.Description)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn).HasColumnName("modified_on");
            entity.Property(e => e.NoticesType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("notices_type");
            entity.Property(e => e.SentBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sent_by");
            entity.Property(e => e.SentOn).HasColumnName("sent_on");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<CsdCourseDetail>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__csd_cour__8F1EF7AEBA43B542");

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
            entity.HasKey(e => e.EmailId).HasName("PK__csd_emai__3FEF8766FA000A24");

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

        modelBuilder.Entity<CsdEventGallery>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__csd_even__2370F727272D0C7B");

            entity.ToTable("csd_event_gallery");

            entity.Property(e => e.EventId)
                .ValueGeneratedNever()
                .HasColumnName("event_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.EventDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("event_description");
            entity.Property(e => e.EventName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("event_name");
        });

        modelBuilder.Entity<CsdEventPhoto>(entity =>
        {
            entity.HasKey(e => e.PhotoId).HasName("PK__csd_even__CB48C83D52FACD09");

            entity.ToTable("csd_event_photo");

            entity.Property(e => e.PhotoId).HasColumnName("photo_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("photo_url");
            entity.Property(e => e.UploadedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("uploaded_by");
            entity.Property(e => e.UploadedDate).HasColumnName("uploaded_date");

            entity.HasOne(d => d.Event).WithMany(p => p.CsdEventPhotos)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FKnqbgrhn5fx37x6imh27d6hmat");
        });

        modelBuilder.Entity<CsdFacultyExperience>(entity =>
        {
            entity.HasKey(e => e.ExperienceId).HasName("PK__csd_facu__EB216AFC7F4936D4");

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
            entity.HasKey(e => e.FacultyId).HasName("PK__csd_facu__7B00413C8DDC3C8E");

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

        modelBuilder.Entity<CsdStudentAttendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__csd_stud__20D6A968F0685D19");

            entity.ToTable("csd_student_attendance");

            entity.Property(e => e.AttendanceId)
                .ValueGeneratedNever()
                .HasColumnName("attendance_id");
            entity.Property(e => e.AttendanceDate)
                .HasPrecision(6)
                .HasColumnName("attendance_date");
            entity.Property(e => e.AttendanceMarked).HasColumnName("attendance_marked");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.QrId).HasColumnName("qr_id");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
        });

        modelBuilder.Entity<CsdStudentChallengeResponse>(entity =>
        {
            entity.HasKey(e => e.ResponseId).HasName("PK__csd_stud__EBECD896365A5B8F");

            entity.ToTable("csd_student_challenge_response");

            entity.Property(e => e.ResponseId)
                .ValueGeneratedNever()
                .HasColumnName("response_id");
            entity.Property(e => e.ChallengeId).HasColumnName("challenge_id");
            entity.Property(e => e.IsSkipped).HasColumnName("is_skipped");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.SelectedOption)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("selected_option");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.SubmittedAt)
                .HasPrecision(6)
                .HasColumnName("submitted_at");

            entity.HasOne(d => d.Challenge).WithMany(p => p.CsdStudentChallengeResponses)
                .HasForeignKey(d => d.ChallengeId)
                .HasConstraintName("FK94noy7q0w8p7ww4a0xli12xte");

            entity.HasOne(d => d.Question).WithMany(p => p.CsdStudentChallengeResponses)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FKr949tfw0rqsmep20pog3yn600");
        });

        modelBuilder.Entity<CsdStudentRegistration>(entity =>
        {
            entity.HasKey(e => e.StdId).HasName("PK__csd_stud__0B0245BA07BCAD8B");

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
            entity.HasKey(e => e.SubjectId).HasName("PK__csd_subj__5004F6602ED4D16D");

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

        modelBuilder.Entity<CsdTeacgerResource>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("PK__csd_teac__4985FC73056E91E6");

            entity.ToTable("csd_teacger_resources");

            entity.Property(e => e.ResourceId)
                .ValueGeneratedNever()
                .HasColumnName("resource_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedOn).HasColumnName("created_on");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.ResourceType)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("resource_type");
            entity.Property(e => e.Semseter).HasColumnName("semseter");
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("subject");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("url");
        });

        modelBuilder.Entity<CsdTeacherAttendanceQr>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__csd_teac__20D6A968E0CDEA25");

            entity.ToTable("csd_teacher_attendance_qr");

            entity.Property(e => e.AttendanceId)
                .ValueGeneratedNever()
                .HasColumnName("attendance_id");
            entity.Property(e => e.AttendanceDate)
                .HasPrecision(6)
                .HasColumnName("attendance_date");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.ExpireTime)
                .HasPrecision(6)
                .HasColumnName("expire_time");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.QrCodeValue)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("qr_code_value");
            entity.Property(e => e.Semester).HasColumnName("semester");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("subject_name");
        });

        modelBuilder.Entity<CsdTeacherChallengeDetail>(entity =>
        {
            entity.HasKey(e => e.ChallengeId).HasName("PK__csd_teac__CF6351917470AEB9");

            entity.ToTable("csd_teacher_challenge_details");

            entity.Property(e => e.ChallengeId)
                .ValueGeneratedNever()
                .HasColumnName("challenge_id");
            entity.Property(e => e.ChallengeName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("challenge_name");
            entity.Property(e => e.CreatedOn).HasColumnName("created_on");
            entity.Property(e => e.DueDate).HasColumnName("due_date");
            entity.Property(e => e.ExamTimeduration).HasColumnName("exam_timeduration");
            entity.Property(e => e.GivenBy)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("given_by");
            entity.Property(e => e.Semester).HasColumnName("semester");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<CsdTeacherChallengePoint>(entity =>
        {
            entity.HasKey(e => e.PointId).HasName("PK__csd_teac__0241361200ADDABF");

            entity.ToTable("csd_teacher_challenge_point");

            entity.Property(e => e.PointId)
                .ValueGeneratedNever()
                .HasColumnName("point_id");
            entity.Property(e => e.ChallengeId).HasColumnName("challenge_id");
            entity.Property(e => e.Point).HasColumnName("point");
            entity.Property(e => e.StdId).HasColumnName("std_id");

            entity.HasOne(d => d.Challenge).WithMany(p => p.CsdTeacherChallengePoints)
                .HasForeignKey(d => d.ChallengeId)
                .HasConstraintName("FKegpm5eue4g76nskruqtrbmx77");
        });

        modelBuilder.Entity<CsdTeacherChallengeQuestion>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__csd_teac__2EC21549B92AFE3D");

            entity.ToTable("csd_teacher_challenge_question");

            entity.Property(e => e.QuestionId)
                .ValueGeneratedNever()
                .HasColumnName("question_id");
            entity.Property(e => e.ChallengeId).HasColumnName("challenge_id");
            entity.Property(e => e.CorrectAnswer)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("correct_answer");
            entity.Property(e => e.OptionList)
                .HasMaxLength(255)
                .HasColumnName("option_list");
            entity.Property(e => e.Questions)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("questions");

            entity.HasOne(d => d.Challenge).WithMany(p => p.CsdTeacherChallengeQuestions)
                .HasForeignKey(d => d.ChallengeId)
                .HasConstraintName("FKjl6gt0u43gkrs9ujrqmysjw4q");
        });

        modelBuilder.Entity<CsdTeacherNoticesDetail>(entity =>
        {
            entity.HasKey(e => e.NoticeId).HasName("PK__csd_teac__3E82A5DBE052CBB5");

            entity.ToTable("csd_teacher_notices_details");

            entity.Property(e => e.NoticeId)
                .ValueGeneratedNever()
                .HasColumnName("notice_id");
            entity.Property(e => e.Body)
                .HasMaxLength(1200)
                .IsUnicode(false)
                .HasColumnName("body");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn).HasColumnName("modified_on");
            entity.Property(e => e.NoticesType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("notices_type");
            entity.Property(e => e.SentBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sent_by");
            entity.Property(e => e.SentOn).HasColumnName("sent_on");
            entity.Property(e => e.Subject)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("subject");
            entity.Property(e => e.TargetAudience)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("target_audience");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<CsdTeacherTimetableDetail>(entity =>
        {
            entity.HasKey(e => e.TimeTableId).HasName("PK__csd_teac__DBDC39B7C13D484D");

            entity.ToTable("csd_teacher_timetable_details");

            entity.Property(e => e.TimeTableId)
                .ValueGeneratedNever()
                .HasColumnName("time_table_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedOn).HasColumnName("created_on");
            entity.Property(e => e.CurrentYear).HasColumnName("current_year");
            entity.Property(e => e.Semester).HasColumnName("semester");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
        });

        modelBuilder.Entity<CsdTeacherTimetableEntry>(entity =>
        {
            entity.HasKey(e => e.EntryId).HasName("PK__csd_teac__810FDCE1D0B340B7");

            entity.ToTable("csd_teacher_timetable_entry");

            entity.Property(e => e.EntryId)
                .ValueGeneratedNever()
                .HasColumnName("entry_id");
            entity.Property(e => e.ClassType)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("class_type");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedOn).HasColumnName("created_on");
            entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
            entity.Property(e => e.RoomNumber).HasColumnName("room_number");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("subject_name");
            entity.Property(e => e.TimeSlot)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("time_slot");
            entity.Property(e => e.TimeTableId).HasColumnName("time_table_id");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");

            entity.HasOne(d => d.TimeTable).WithMany(p => p.CsdTeacherTimetableEntries)
                .HasForeignKey(d => d.TimeTableId)
                .HasConstraintName("FKnabnr28raup2wcwktg7hrpnlg");
        });

        modelBuilder.Entity<CsdUserRegistration>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__csd_user__B9BE370F89556190");

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

        modelBuilder.Entity<FoCourseDetail>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__fo_cours__8F1EF7AEE1AE50A0");

            entity.ToTable("fo_course_details");

            entity.Property(e => e.CourseId)
                .ValueGeneratedNever()
                .HasColumnName("course_id");
            entity.Property(e => e.CourseName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("course_name");
            entity.Property(e => e.Duration)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("duration");
            entity.Property(e => e.Fees).HasColumnName("fees");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
        });

        modelBuilder.Entity<FoEnquiryDetail>(entity =>
        {
            entity.HasKey(e => e.EnquiryId).HasName("PK__fo_enqui__57CC01B31BAD5633");

            entity.ToTable("fo_enquiry_details");

            entity.Property(e => e.EnquiryId)
                .ValueGeneratedNever()
                .HasColumnName("enquiry_id");
            entity.Property(e => e.ClassMode)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("class_mode");
            entity.Property(e => e.CourseName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("course_name");
            entity.Property(e => e.EnquiryDate).HasColumnName("enquiry_date");
            entity.Property(e => e.EnquiryStatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("enquiry_status");
            entity.Property(e => e.StudPhoneNumber).HasColumnName("stud_phone_number");
            entity.Property(e => e.StudentMailId)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("student_mail_id");
            entity.Property(e => e.StudentName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("student_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.FoEnquiryDetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK4hvq4e5dc2v1xp8lh7hmxm4r3");
        });

        modelBuilder.Entity<FoEnquiryStatus>(entity =>
        {
            entity.HasKey(e => e.EnquiryId).HasName("PK__fo_enqui__57CC01B3E4B65E39");

            entity.ToTable("fo_enquiry_status");

            entity.Property(e => e.EnquiryId)
                .ValueGeneratedNever()
                .HasColumnName("enquiry_id");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<FoUserAccountDetail>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__fo_user___B9BE370F9635A109");

            entity.ToTable("fo_user_account_details");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.AccountStatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("account_status");
            entity.Property(e => e.MailId)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("mail_id");
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
            entity.Property(e => e.UserName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<FopCourseDetail>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__fop_cour__8F1EF7AEAFB9347D");

            entity.ToTable("fop_course_details");

            entity.Property(e => e.CourseId)
                .ValueGeneratedNever()
                .HasColumnName("course_id");
            entity.Property(e => e.CourseName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("course_name");
            entity.Property(e => e.Duration)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("duration");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
        });

        modelBuilder.Entity<FopEnquiryDetail>(entity =>
        {
            entity.HasKey(e => e.EnquiryId).HasName("PK__fop_enqu__57CC01B305B6A127");

            entity.ToTable("fop_enquiry_details");

            entity.Property(e => e.EnquiryId)
                .ValueGeneratedNever()
                .HasColumnName("enquiry_id");
            entity.Property(e => e.ClassMode)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("class_mode");
            entity.Property(e => e.CourseName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("course_name");
            entity.Property(e => e.EnquiryDate).HasColumnName("enquiry_date");
            entity.Property(e => e.EnquiryStatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("enquiry_status");
            entity.Property(e => e.StudPhoneNumber).HasColumnName("stud_phone_number");
            entity.Property(e => e.StudentMailId)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("student_mail_id");
            entity.Property(e => e.StudentName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("student_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.FopEnquiryDetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FKkx3057vq0egfmbv57m80orw0p");
        });

        modelBuilder.Entity<FopEnquiryStatus>(entity =>
        {
            entity.HasKey(e => e.EnquiryId).HasName("PK__fop_enqu__57CC01B3FF2D2125");

            entity.ToTable("fop_enquiry_status");

            entity.Property(e => e.EnquiryId)
                .ValueGeneratedNever()
                .HasColumnName("enquiry_id");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<FopUserAccountDetail>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__fop_user__B9BE370F014A0683");

            entity.ToTable("fop_user_account_details");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.AccountStatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("account_status");
            entity.Property(e => e.MailId)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("mail_id");
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
            entity.Property(e => e.UserName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<InternalMarkDetailsEntity>(entity =>
        {
            entity.HasKey(e => e.MarkId).HasName("PK__internal__61D223B576E98C8D");

            entity.ToTable("internal_mark_details_entity");

            entity.Property(e => e.MarkId)
                .ValueGeneratedNever()
                .HasColumnName("mark_id");
            entity.Property(e => e.MarkSecured).HasColumnName("mark_secured");
            entity.Property(e => e.PublishedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("published_by");
            entity.Property(e => e.PublishedDate).HasColumnName("published_date");
            entity.Property(e => e.RollNumber).HasColumnName("roll_number");
            entity.Property(e => e.SecuredLabMark).HasColumnName("secured_lab_mark");
            entity.Property(e => e.SecuredWrittenMark).HasColumnName("secured_written_mark");
            entity.Property(e => e.Semester).HasColumnName("semester");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("subject_name");
            entity.Property(e => e.TotalExamMark).HasColumnName("total_exam_mark");
            entity.Property(e => e.TotalLabMark).HasColumnName("total_lab_mark");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
        });

        modelBuilder.Entity<MailDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mail_det__3213E83FF08DD3F8");

            entity.ToTable("mail_details");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.DraftName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("draft_name");
            entity.Property(e => e.LastBatchSentAt)
                .HasPrecision(6)
                .HasColumnName("last_batch_sent_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Path)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("path");
            entity.Property(e => e.ResumeContentType)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("resume_content_type");
            entity.Property(e => e.ResumeFileName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("resume_file_name");
            entity.Property(e => e.ResumePath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("resume_path");
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("subject");
            entity.Property(e => e.TotalMailsSent).HasColumnName("total_mails_sent");
        });
        modelBuilder.HasSequence("csd_admin_registration_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_alumni_details_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_amdin_notices_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_course_details_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_email_validation_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_event_gallery_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_student_attendance_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_student_challenge_response_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_subject_details_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_teacger_resources_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_teacher_attendance_qr_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_teacher_challenge_details_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_teacher_challenge_point_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_teacher_notices_details_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_teacher_timetable_details_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd_teacher_timetable_entry_seq").IncrementsBy(50);
        modelBuilder.HasSequence("csd-question-seq");
        modelBuilder.HasSequence("fo_course_seq1").StartsAt(100L);
        modelBuilder.HasSequence("fo_enquiry_seq1").StartsAt(10000L);
        modelBuilder.HasSequence("fo_enquirysts_seq1");
        modelBuilder.HasSequence("fo_ua_seq1").StartsAt(1001L);
        modelBuilder.HasSequence("mail_details_seq").IncrementsBy(50);
        modelBuilder.HasSequence("seq1").StartsAt(1001L);
        modelBuilder.HasSequence("seq2").StartsAt(10001L);
        modelBuilder.HasSequence("seq3").StartsAt(100L);
        modelBuilder.HasSequence("seq4").StartsAt(50L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
