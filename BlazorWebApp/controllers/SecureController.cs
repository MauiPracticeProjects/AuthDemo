using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebApp.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class SecureController : ControllerBase
    {
        [HttpGet("ProtectedData")]
        [Authorize]
        public IActionResult GetProtectedData()
        {
            return Ok(new { Message = "You accessed a protected endpoint!" });
        }
    }
}
