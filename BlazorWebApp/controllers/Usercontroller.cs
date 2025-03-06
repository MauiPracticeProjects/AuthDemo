using BlazorWebApp.Interface;
using BlazorWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebApp.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Usercontroller : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public Usercontroller(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> UserRegistration(UserModel model)
        {
            var result = await _userRepository.UserRegistration(model);
            return Ok(result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUser(UserLoginModel model)
        {
            var result = await _userRepository.LoginUser(model);
            var tokenProperty = result.Result.GetType().GetProperty("Token");
            var token = tokenProperty?.GetValue(result.Result)?.ToString();
            return Ok(token);
        }
    }
}
