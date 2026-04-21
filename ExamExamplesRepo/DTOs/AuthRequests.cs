namespace WebAPI.Dtos
{
    public record LoginRequestDto(string Username, string Password);
    public record RefreshRequestDto(string RefreshToken);
    public record RegisterRequestDto(string Username, string Password, string Role);
    public record RevokeRequestDto(string RefreshToken);
}
