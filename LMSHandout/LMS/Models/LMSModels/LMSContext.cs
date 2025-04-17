using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LMS.Models.LMSModels
{
    public partial class LMSContext : DbContext
    {
        public LMSContext()
        {
        }

        public LMSContext(DbContextOptions<LMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrator> Administrators { get; set; } = null!;
        public virtual DbSet<Assignment> Assignments { get; set; } = null!;
        public virtual DbSet<AssignmentCategory> AssignmentCategories { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Enrolled> Enrolleds { get; set; } = null!;
        public virtual DbSet<Professor> Professors { get; set; } = null!;
        public virtual DbSet<Sshkey> Sshkeys { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Submission> Submissions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("name=LMS:LMSConnectionString", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.8-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("latin1_swedish_ci")
                .HasCharSet("latin1");

            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.HasKey(e => e.AdministratorNumber)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Uid, "uid")
                    .IsUnique();

                entity.Property(e => e.AdministratorNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("administratorNumber");

                entity.Property(e => e.Dob).HasColumnName("dob");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .HasColumnName("lastName");

                entity.Property(e => e.Uid)
                    .HasMaxLength(8)
                    .HasColumnName("uid")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.HasKey(e => e.AssignmentNumber)
                    .HasName("PRIMARY");

                entity.HasIndex(e => new { e.CategoryNumber, e.Name }, "uniqueAssignment")
                    .IsUnique();

                entity.Property(e => e.AssignmentNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("assignmentNumber");

                entity.Property(e => e.CategoryNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("categoryNumber");

                entity.Property(e => e.Contents)
                    .HasMaxLength(8192)
                    .HasColumnName("contents");

                entity.Property(e => e.DueDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("dueDateTime");

                entity.Property(e => e.MaxPoints)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("maxPoints");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.HasOne(d => d.CategoryNumberNavigation)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.CategoryNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Assignments_Category");
            });

            modelBuilder.Entity<AssignmentCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryNumber)
                    .HasName("PRIMARY");

                entity.HasIndex(e => new { e.CourseNumber, e.Name }, "uniqueAssignmentCategory")
                    .IsUnique();

                entity.Property(e => e.CategoryNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("categoryNumber");

                entity.Property(e => e.CourseNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("courseNumber");

                entity.Property(e => e.GradingWeight)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("gradingWeight");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => new { e.CourseNumber, e.Year, e.Season })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.Property(e => e.CourseNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("courseNumber");

                entity.Property(e => e.Year)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("year");

                entity.Property(e => e.Season)
                    .HasColumnType("enum('Spring','Summer','Fall')")
                    .HasColumnName("season");

                entity.Property(e => e.EndTime)
                    .HasColumnType("time")
                    .HasColumnName("endTime");

                entity.Property(e => e.Location)
                    .HasMaxLength(100)
                    .HasColumnName("location");

                entity.Property(e => e.StartTime)
                    .HasColumnType("time")
                    .HasColumnName("startTime");

                entity.HasOne(d => d.CourseNumberNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.CourseNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Classes_ibfk_1");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseNumber)
                    .HasName("PRIMARY");

                entity.HasIndex(e => new { e.SubjectAbbreviation, e.Number }, "uniqueCourse")
                    .IsUnique();

                entity.Property(e => e.CourseNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("courseNumber");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Number)
                    .HasColumnType("int(11)")
                    .HasColumnName("number");

                entity.Property(e => e.SubjectAbbreviation)
                    .HasMaxLength(4)
                    .HasColumnName("subjectAbbreviation");

                entity.HasOne(d => d.SubjectAbbreviationNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.SubjectAbbreviation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Courses_ibfk_1");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.SubjectAbbreviation)
                    .HasName("PRIMARY");

                entity.Property(e => e.SubjectAbbreviation).HasMaxLength(4);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Enrolled>(entity =>
            {
                entity.HasKey(e => e.EnrollmentNumber)
                    .HasName("PRIMARY");

                entity.ToTable("Enrolled");

                entity.HasIndex(e => e.StudentNumber, "studentNumber");

                entity.HasIndex(e => new { e.CourseNumber, e.StudentNumber }, "uniqueEnrollment")
                    .IsUnique();

                entity.Property(e => e.EnrollmentNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("enrollmentNumber");

                entity.Property(e => e.CourseNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("courseNumber");

                entity.Property(e => e.Grade)
                    .HasMaxLength(2)
                    .HasColumnName("grade");

                entity.Property(e => e.StudentNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("studentNumber");

                entity.HasOne(d => d.CourseNumberNavigation)
                    .WithMany(p => p.Enrolleds)
                    .HasForeignKey(d => d.CourseNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Enrolled_ibfk_2");

                entity.HasOne(d => d.StudentNumberNavigation)
                    .WithMany(p => p.Enrolleds)
                    .HasForeignKey(d => d.StudentNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Enrolled_ibfk_1");
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.ProfessorNumber)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Uid, "uid")
                    .IsUnique();

                entity.Property(e => e.ProfessorNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("professorNumber");

                entity.Property(e => e.Dob).HasColumnName("dob");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .HasColumnName("lastName");

                entity.Property(e => e.Uid)
                    .HasMaxLength(8)
                    .HasColumnName("uid")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Sshkey>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sshkey");

                entity.Property(e => e.Sshkey1)
                    .HasColumnType("text")
                    .HasColumnName("sshkey");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentNumber)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Major, "major");

                entity.HasIndex(e => e.Uid, "uid")
                    .IsUnique();

                entity.Property(e => e.StudentNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("studentNumber");

                entity.Property(e => e.Dob).HasColumnName("dob");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .HasColumnName("lastName");

                entity.Property(e => e.Major)
                    .HasMaxLength(4)
                    .HasColumnName("major");

                entity.Property(e => e.Uid)
                    .HasMaxLength(8)
                    .HasColumnName("uid")
                    .IsFixedLength();

                entity.HasOne(d => d.MajorNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.Major)
                    .HasConstraintName("Students_ibfk_1");
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.HasKey(e => e.SubmissionNumber)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.StudentNumber, "studentNumber");

                entity.HasIndex(e => new { e.AssignmentNumber, e.StudentNumber }, "uniqueSubmission")
                    .IsUnique();

                entity.Property(e => e.SubmissionNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("submissionNumber");

                entity.Property(e => e.AssignmentNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("assignmentNumber");

                entity.Property(e => e.Contents)
                    .HasMaxLength(8192)
                    .HasColumnName("contents");

                entity.Property(e => e.Score)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("score");

                entity.Property(e => e.StudentNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("studentNumber");

                entity.Property(e => e.SubmittedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("submittedAt");

                entity.HasOne(d => d.AssignmentNumberNavigation)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.AssignmentNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Submissions_ibfk_1");

                entity.HasOne(d => d.StudentNumberNavigation)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.StudentNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Submissions_ibfk_2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
