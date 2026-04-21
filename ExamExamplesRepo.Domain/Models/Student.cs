using System;
using System.Collections.Generic;

namespace ExamExamplesRepo.Infrastruture.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Gender { get; set; }

    public int? Paid { get; set; }

    public string? Email { get; set; }

    public string? City { get; set; }

    public int? GodparentId { get; set; }

    public virtual Student? Godparent { get; set; }

    public virtual ICollection<Student> InverseGodparent { get; set; } = new List<Student>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
