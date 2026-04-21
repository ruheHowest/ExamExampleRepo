using System;
using System.Collections.Generic;
using ExamExamplesRepo.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamExamplesRepo.Infrastruture.Data;

public partial class SchoolDbContext : DbContext
{
    public SchoolDbContext()
    {
    }

    public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentsOld> StudentsOlds { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<VStudentsWithCourse> VStudentsWithCourses { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Name).IsUnique();
            entity.Property(u => u.Name).HasMaxLength(100).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(rt => rt.Id);
            entity.HasOne(rt => rt.User)
                  .WithMany(u => u.RefreshTokens)
                  .HasForeignKey(rt => rt.UserId);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("pk_courses");

            entity.ToTable("courses");

            entity.Property(e => e.CourseId).HasColumnName("courseid");
            entity.Property(e => e.CourseName)
                .HasMaxLength(30)
                .HasColumnName("coursename");
            entity.Property(e => e.Fee).HasColumnName("fee");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("pk_students");

            entity.ToTable("students");

            entity.Property(e => e.StudentId).HasColumnName("studentid");
            entity.Property(e => e.BirthDate).HasColumnName("birthdate");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .HasColumnName("firstname");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.GodparentId).HasColumnName("godparent_id");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .HasColumnName("lastname");
            entity.Property(e => e.Paid).HasColumnName("paid");

            entity.HasOne(d => d.Godparent).WithMany(p => p.InverseGodparent)
                .HasForeignKey(d => d.GodparentId)
                .HasConstraintName("fk_godparent");

            entity.HasMany(d => d.Courses).WithMany(p => p.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentsCourse",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("Courseid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk2_students_courses"),
                    l => l.HasOne<Student>().WithMany()
                        .HasForeignKey("Studentid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk1_students_courses"),
                    j =>
                    {
                        j.HasKey("Studentid", "Courseid").HasName("pk_students_courses");
                        j.ToTable("students_courses");
                        j.IndexerProperty<int>("Studentid").HasColumnName("studentid");
                        j.IndexerProperty<int>("Courseid").HasColumnName("courseid");
                    });
        });

        modelBuilder.Entity<StudentsOld>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("studentsOld");

            entity.Property(e => e.BirthDate).HasColumnName("birthdate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .HasColumnName("firstname");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .HasColumnName("lastname");
            entity.Property(e => e.Paid).HasColumnName("paid");
            entity.Property(e => e.StudentId).HasColumnName("studentid");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__teachers__98EA44AD727352DE");

            entity.ToTable("teachers");

            entity.Property(e => e.TeacherId).HasColumnName("teacherid");
            entity.Property(e => e.CourseId).HasColumnName("courseid");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("lastname");

            entity.HasOne(d => d.Course).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__teachers__course__6EF57B66");
        });

        modelBuilder.Entity<VStudentsWithCourse>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vStudentsWithCourses");

            entity.Property(e => e.BirthDate).HasColumnName("birthdate");
            entity.Property(e => e.CourseId).HasColumnName("courseid");
            entity.Property(e => e.CourseName)
                .HasMaxLength(30)
                .HasColumnName("coursename");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Fee).HasColumnName("fee");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .HasColumnName("firstname");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .HasColumnName("lastname");
            entity.Property(e => e.Paid).HasColumnName("paid");
            entity.Property(e => e.StudentId).HasColumnName("studentid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
