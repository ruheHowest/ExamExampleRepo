using ExamExamplesRepo.Domain.DTOs;
using ExamExamplesRepo.Domain.Interfaces;
using ExamExamplesRepo.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ExamExamplesRepo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController: ControllerBase
    {
        private readonly IStudentRepository _repo;

        public StudentsController(IStudentRepository repo) => _repo = repo;

        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> GetStudents(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            CancellationToken ct = default
        )
        {
            var (students, total) = await _repo.GetPagedStudentsAsync(page, size, ct);
            var response = new PagedResponse<StudentDto>(
                students.ToDtos(),
                total,
                page,
                size
            );

            return Ok(response);
        }
    }
}
