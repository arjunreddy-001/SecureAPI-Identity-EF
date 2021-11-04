using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecureAPI.Models;

namespace SecureAPI_Identity_EF.Models
{
    public class AuthUserContext : IdentityDbContext<AuthUser, IdentityRole, string>
    {
        public AuthUserContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}