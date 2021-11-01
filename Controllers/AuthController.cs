using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecureAPI.Models;
using SecureAPI.Models.BindingModels;
using SecureAPI.Models.DTOs;
using SecureAPI.Services;
using SecureAPI_Identity_EF.Models.BindingModels;

namespace SecureAPI_Identity_EF.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authSvc;

        public AuthController(IAuthService authService)
        {
            _authSvc = authService;
        }

        [HttpPost("register")]
        public async Task<object> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model) 
        {
            try
            {
                var user = new AuthUser(){FullName = model.FullName, Email = model.Email, UserName = model.Email, DateCreated = DateTime.UtcNow, DateModified = DateTime.UtcNow};
                var result = await _authSvc.CreateUser(user, model.Password);

                if(result.Succeeded)
                {
                    return await Task.FromResult("User has been registered");
                }

                return await Task.FromResult(
                    string.Join(",", result.Errors.Select(x => x.Description).ToArray())
                );
            } 
            catch(Exception ex)
            {
                return await Task.FromResult(ex.Message);
            } 
        }
    
        [HttpPost("login")]
        public async Task<object> Login([FromBody] LoginBindingModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var result = await _authSvc.Authenticate(model.Email, model.Password);

                    if(result.Succeeded)
                    {
                        var authUser = await _authSvc.GetUserByEmail(model.Email);
                        var user = new AuthUserDTO(authUser.FullName, authUser.Email, authUser.UserName, authUser.DateCreated);
                        user.Token = _authSvc.GenerateToken(authUser);

                        return await Task.FromResult(user);
                    }
                }

                return await Task.FromResult("invalid email or password");
            }
            catch(Exception ex)
            {
                return await Task.FromResult(ex.Message);
            }
        }
    }
}