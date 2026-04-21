using ExamExamplesRepo.Domain.Models;

namespace ExamExamplesRepo.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken(int userId);
    }
}
