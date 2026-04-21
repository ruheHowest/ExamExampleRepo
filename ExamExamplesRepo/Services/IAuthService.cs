using WebAPI.Dtos;

namespace ExamExamplesRepo.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<AuthResponseDto> RefreshAsync(string refreshToken);
        Task RevokeAsync(string refreshToken);
        Task RevokeAllAsync(int userId);
    }
}
