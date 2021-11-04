using System;
using Microsoft.AspNetCore.Identity;

namespace SecureAPI.Models
{
    public class AuthUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}