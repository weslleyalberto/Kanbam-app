
using Kabam.api.Service;
using Kabam.api.VM;
using Kabam.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kabam.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<User> _signMananger;
        private readonly UserManager<User> _userManager;

        public LoginController(SignInManager<User> signMananger, UserManager<User> userManager)
        {
            _signMananger = signMananger;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
                var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized("Usuário ou senha inválidos.");

            var result = await _signMananger.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Usuário ou senha inválidos.");
            // Gerar o token JWT
            var token = GenerateTokenService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
    }
}
