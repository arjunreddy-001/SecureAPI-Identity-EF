using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SecureAPI.Models;

namespace SecureAPI.Services
{
    public interface IAuthService
    {
        Task<SignInResult> Authenticate(string email, string password);
        string GenerateToken(AuthUser user);
        Task<IdentityResult> CreateUser(AuthUser user, string password);
        Task<AuthUser> GetUserByEmail(string email);
    }
}