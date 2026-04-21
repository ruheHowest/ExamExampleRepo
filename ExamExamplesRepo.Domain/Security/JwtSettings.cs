using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExamplesRepo.Domain.Security
{
    public class JwtSettings
    {
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public string SecretKey { get; init; } = string.Empty;
        public int AccessTokenExpiryMinutes { get; init; }
        public int RefreshTokenExpiryDays { get; init; }
    }
}
