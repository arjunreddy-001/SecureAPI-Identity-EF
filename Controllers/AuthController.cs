using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecureAPI.Models;
using SecureAPI.Models.BindingModels;
using SecureAPI.Models.DTOs;
using SecureAPI.Services;
using SecureAPI_Identity_EF.Models.BindingModels;
using SecureAPI_Identity_EF.Models.Response;

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
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "User has been registered", null));
                }  

                return await Task.FromResult(
                    new ResponseModel(ResponseCode.Error, "", string.Join(",", result.Errors.Select(x => x.Description).ToArray()))
                );
            } 
            catch(Exception ex)             
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
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

                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "User authenticated", user));
                    }
                }

                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Invalid email or password", null));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
    }
}