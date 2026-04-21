using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExamplesRepo.Domain.Models
{
    public static class Roles
    {
        public const string Admin = "Admin";   // Full access - manage users, all data
        public const string Manager = "Manager"; // Manage data, view reports
        public const string User = "User";    // Read and edit own data
    }
}
