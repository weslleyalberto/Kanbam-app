using Kabam.api.Service;
using Kabam.Domain;
using Microsoft.AspNetCore.Identity;

namespace Kabam.api.Endpoints
{
    public static class LoginEndpoints
    {
        public static IEndpointRouteBuilder MapLoginEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/api/login/login", async (
                LoginModel model,
                SignInManager<User> signInManager,
                UserManager<User> userManager) =>
            {
                // Validação simples (opcional: use MiniValidation ou outra estratégia)
                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                {
                    return Results.BadRequest("Dados inválidos.");
                }

                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return Results.Unauthorized();

                var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded)
                    return Results.Unauthorized();

                // Gerar o token JWT
                var token = GenerateTokenService.GenerateJwtToken(user);

                return Results.Ok(new LoginResult
                {
                    Successful = true,
                    Token = token
                });
            });

            return endpoints;
        }
    }
}

