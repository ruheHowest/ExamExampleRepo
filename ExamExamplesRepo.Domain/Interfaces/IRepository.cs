using ExamExamplesRepo.Domain.Models;
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
        Task<(IEnumerable<Student> Students, int Total)> GetPagedStudentsAsync(int page, int pageSize, CancellationToken ct);
    }

    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<RefreshToken> AddAsync(RefreshToken refreshToken);
        Task RevokeAsync(RefreshToken refreshToken);
        Task RevokeAllForUserAsync(int userId);
    }

    public interface ITokenRepository
    {
        string GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken(int userId);
    }

    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> UsernameExistsAsync(string username);
        Task<User> AddAsync(User user);
    }
}
