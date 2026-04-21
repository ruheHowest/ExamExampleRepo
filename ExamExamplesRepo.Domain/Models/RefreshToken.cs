using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExamplesRepo.Domain.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        // RandomNumberGenerator.GetBytes(64)
        public string Token { get; set; } = string.Empty; // random base64 string
        public DateTime CreatedAt { get; set; } // When was token issued
        public DateTime ExpiresAt { get; set; } // Expire date: default is 7 days
        public bool IsRevoked { get; set; } // set true to invalidate
        public int UserId { get; set; }
        public User User { get; set; } = null;

        // Computed
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
