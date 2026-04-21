using System;
using System.Collections.Generic;

namespace ExamExamplesRepo.Infrastruture.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public int? CourseId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public virtual Course? Course { get; set; }
}
