using ExamExamplesRepo.Domain.DTOs;
using ExamExamplesRepo.Domain.Models;

namespace ExamExamplesRepo.Extensions
{
    public static class StudentMappingExtensions
    {
        public static StudentDto ToDto(this Student student)
        {
            return new StudentDto(
                student.StudentId,
                $"{student.FirstName} {student.LastName}",
                student.Courses
                    .Select(sc => sc.CourseName ?? "Onbekende cursus")
                    .ToList()
            );
        }

        public static IEnumerable<StudentDto> ToDtos(this IEnumerable<Student> students)
        {
            return students.Select(s => s.ToDto());
        }
    }
}
