using System;
using System.Collections.Generic;
using ExamExamplesRepo.Infrastruture.Models;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=school;Integrated Security=SSPI;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Courseid).HasName("pk_courses");

            entity.ToTable("courses");

            entity.Property(e => e.Courseid).HasColumnName("courseid");
            entity.Property(e => e.Coursename)
                .HasMaxLength(30)
                .HasColumnName("coursename");
            entity.Property(e => e.Fee).HasColumnName("fee");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Studentid).HasName("pk_students");

            entity.ToTable("students");

            entity.Property(e => e.Studentid).HasColumnName("studentid");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(30)
                .HasColumnName("firstname");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.GodparentId).HasColumnName("godparent_id");
            entity.Property(e => e.Lastname)
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

            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(30)
                .HasColumnName("firstname");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.Lastname)
                .HasMaxLength(30)
                .HasColumnName("lastname");
            entity.Property(e => e.Paid).HasColumnName("paid");
            entity.Property(e => e.Studentid).HasColumnName("studentid");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Teacherid).HasName("PK__teachers__98EA44AD727352DE");

            entity.ToTable("teachers");

            entity.Property(e => e.Teacherid).HasColumnName("teacherid");
            entity.Property(e => e.Courseid).HasColumnName("courseid");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");

            entity.HasOne(d => d.Course).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.Courseid)
                .HasConstraintName("FK__teachers__course__6EF57B66");
        });

        modelBuilder.Entity<VStudentsWithCourse>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vStudentsWithCourses");

            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Courseid).HasColumnName("courseid");
            entity.Property(e => e.Coursename)
                .HasMaxLength(30)
                .HasColumnName("coursename");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Fee).HasColumnName("fee");
            entity.Property(e => e.Firstname)
                .HasMaxLength(30)
                .HasColumnName("firstname");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.Lastname)
                .HasMaxLength(30)
                .HasColumnName("lastname");
            entity.Property(e => e.Paid).HasColumnName("paid");
            entity.Property(e => e.Studentid).HasColumnName("studentid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
