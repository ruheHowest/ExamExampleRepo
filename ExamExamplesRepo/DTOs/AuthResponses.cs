namespace WebAPI.Dtos
{
    public record AuthResponseDto(string AccessToken, string RefreshToken, DateTime ExpiresAt);
    public record IsAdminResponseDto(bool IsAdmin);
    public record ProfileResponseDto(int Id, string Username, string Role);
}
