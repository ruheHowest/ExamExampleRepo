using ExamExamplesRepo.Domain.Interfaces;
using ExamExamplesRepo.Domain.Models;
using ExamExamplesRepo.Infrastruture.Data;
using ExamExamplesRepo.Infrastruture.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExamplesRepo.Infrastruture.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolDbContext _context;

        public StudentRepository(SchoolDbContext context) => _context = context;

        public async Task<IEnumerable<Student>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Students
                .OrderBy(s => s.StudentId)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Student?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Students
                .Where(s => s.StudentId == id)
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);
        }

        public async Task<(IEnumerable<Student> Students, int Total)> GetPagedStudentsAsync(int page, int pageSize, CancellationToken ct)
        {
            IQueryable<Student> query = _context.Students
                .Include(s => s.Courses)
                .AsNoTracking();

            int total = await query.CountAsync();

            var students = await query
                .OrderBy(s => s.LastName)
                .ToPagedList(page, pageSize)
                .ToListAsync(ct);

            return (Students: students, Total: total);
        }

        public IEnumerable<string> GetStudentNamesStream(IEnumerable<Student> students)
        {
            foreach (var student in students)
            {
                if (student.Email is not null) yield return $"{student.FirstName} {student.LastName}";
            }
        }
    }
}
