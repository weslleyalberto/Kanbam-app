using Kabam.api.Data;
using Kabam.api.Service;
using Kabam.api.VM;
using Kabam.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kabam.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly UserManager<User> _userMananger;

        public RegisterController(UserContext context, UserManager<User> userMananger)
        {
            _context = context;
            _userMananger = userMananger;
        }
        [HttpPost("create")]
        public async Task<ActionResult> Add([FromBody] RegisterVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userMananger.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Opcional: gerar token JWT para o usuário recém-registrado
            var token = GenerateTokenService.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }
    }
}
