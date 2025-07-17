using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        [HttpPost("registrar-dispositivo")]
        public IActionResult IniciarSessao()
        {
            var sessionToken = Guid.NewGuid().ToString("N");
            return Ok(new { token = sessionToken });
        }
    }
}