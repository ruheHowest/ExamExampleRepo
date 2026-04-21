using System;
using System.Collections.Generic;

namespace ExamExamplesRepo.Domain.Models;

public partial class VStudentsWithCourse
{
    public int? StudentId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Gender { get; set; }

    public int? Paid { get; set; }

    public string? Email { get; set; }

    public int? CourseId { get; set; }

    public string? CourseName { get; set; }

    public int? Fee { get; set; }
}
