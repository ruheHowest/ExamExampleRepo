using ExamExamplesRepo.Domain.Interfaces;
using ExamExamplesRepo.Domain.Models;
using ExamExamplesRepo.Domain.Security;
using Microsoft.Extensions.Options;
using WebAPI.Dtos;

namespace ExamExamplesRepo.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService,
            IOptions<JwtSettings> jwtOptions)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task RegisterAsync(RegisterRequestDto request)
        {
            bool usernameExists = await _userRepository.UsernameExistsAsync(request.Username);

            if (usernameExists)
            {
                throw new InvalidOperationException($"Username '{request.Username}' is already taken.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            User user = new User
            {
                Name = request.Username,
                PasswordHash = passwordHash,
                Role = request.Role
            };

            await _userRepository.AddAsync(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            User? user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials.");

            string accessToken = _tokenService.GenerateAccessToken(user);
            RefreshToken refreshToken = _tokenService.GenerateRefreshToken(user.Id);

            await _refreshTokenRepository.AddAsync(refreshToken);

            return new AuthResponseDto(
                AccessToken: accessToken,
                RefreshToken: refreshToken.Token,
                ExpiresAt: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes)
            );
        }

        public async Task<AuthResponseDto> RefreshAsync(string refreshToken)
        {
            RefreshToken? storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken is null || !storedToken.IsActive)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            User user = storedToken.User;

            string newAccessToken = _tokenService.GenerateAccessToken(user);

            await _refreshTokenRepository.RevokeAsync(storedToken);

            RefreshToken newRefreshToken = _tokenService.GenerateRefreshToken(user.Id);
            await _refreshTokenRepository.AddAsync(newRefreshToken);

            return new AuthResponseDto(
                AccessToken: newAccessToken,
                RefreshToken: newRefreshToken.Token,
                ExpiresAt: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes)
            );
        }

        public async Task RevokeAsync(string refreshTokenValue)
        {
            RefreshToken? token =
                await _refreshTokenRepository.GetByTokenAsync(refreshTokenValue);

            if (token is null || !token.IsActive)
                throw new UnauthorizedAccessException("Token not found or already revoked.");

            await _refreshTokenRepository.RevokeAsync(token);
        }

        public async Task RevokeAllAsync(int userId)
            => await _refreshTokenRepository.RevokeAllForUserAsync(userId);
    }
}
