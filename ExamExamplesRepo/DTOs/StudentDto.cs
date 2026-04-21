using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExamplesRepo.Domain.DTOs
{
    public record StudentDto(
        int Id,
        string FullName,
        IEnumerable<string> Courses
    );

    public record PagedResponse<T>(
        IEnumerable<T> Data,
        int TotalCount,
        int PageNumber,
        int PageSize
    )
    {
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
