using Kabam.Domain;
using Microsoft.AspNetCore.Identity;

namespace Kabam.api.Endpoints
{
    public static class RegisterEndpoints
    {
        public static IEndpointRouteBuilder MapRegisterEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/api/register/create", async (RegisterModel model, UserManager<User> userManager) =>
            {
                // Validação básica (opcional: utilize MiniValidation para validação automática)
                if (model is null)
                {
                    return Results.BadRequest("Dados inválidos.");
                }

                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(x => x.Description);
                    return Results.Ok(new RegisterResult { Successful = false, Errors = errors });
                }

                // Opcional: gerar token JWT para o usuário recém-registrado
                // var token = GenerateTokenService.GenerateJwtToken(user);

                return Results.Ok(new RegisterResult
                {
                    Successful = true,
                });
            });

            return endpoints;
        }
    }
}
