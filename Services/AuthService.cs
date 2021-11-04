using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecureAPI.Models;

namespace SecureAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signManager;

        public AuthService(UserManager<AuthUser> userManager, SignInManager<AuthUser> signInManager, IOptions<JwtConfig> jwtConfig)
        {
            _userManager = userManager;
            _signManager = signInManager;
            _jwtConfig = jwtConfig.Value;
        }

        public async Task<SignInResult> Authenticate(string email, string password)
        {
            var result = await _signManager.PasswordSignInAsync(email, password, false, false);
            return result;
        }

        public async Task<IdentityResult> CreateUser(AuthUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public string GenerateToken(AuthUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        public async Task<AuthUser> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }
    }
}