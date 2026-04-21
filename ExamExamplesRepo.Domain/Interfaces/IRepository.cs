using ExamExamplesRepo.Infrastruture.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExamplesRepo.Domain.Interfaces
{
    public interface IRepository<T> where T: class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct);
        Task<T?> GetByIdAsync(int id, CancellationToken ct);
    }

    public interface IStudentRepository
        : IRepository<Student>
    {
        Task<(IEnumerable<Student> Students, int Total)> GetPagesStudentsAsync(int page, int pageSize, CancellationToken ct);
    }
}
