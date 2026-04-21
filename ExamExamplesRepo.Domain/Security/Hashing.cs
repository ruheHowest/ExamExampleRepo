using System;
using System.Collections.Generic;
using System.Text;


namespace ExamExamplesRepo.Domain.Security
{
    public static class Hashing
    {
        public static string Hash(string s) => BCrypt.Net.BCrypt.HashPassword(s);
        public static bool Verify(string input, string hash) => BCrypt.Net.BCrypt.Verify(input, hash);
    }
}
