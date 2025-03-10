using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebApp.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SecureController : ControllerBase
    {
        [HttpGet("ProtectedData")]
        
        public IActionResult GetProtectedData()
        {
            return Ok(new { Message = "You accessed a protected endpoint!" });
        }
    }
}
