using Kabam.api.VM;
using Kabam.Domain;
using Kabam.Domain.Interfaces;
using System.Security.Claims;

namespace Kabam.api.Endpoints
{
    public static class TarefasEndpoints
    {
        public static IEndpointRouteBuilder MapTarefasEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // Cria um grupo de endpoints com a rota base e exige autorização
            var group = endpoints.MapGroup("/api/tarefas").RequireAuthorization();

            // GET /api/tarefas/getall
            group.MapGet("/getall", async (HttpContext httpContext, ITarefaDomain tarefaDomain) =>
            {
                // Recupera o userId a partir do token (claim)
                var userId = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                var result = await tarefaDomain.GetAll(userId);
                return result is null ? Results.NoContent() : Results.Ok(result);
            });

            // GET /api/tarefas/getbyid?idTarefa=valor
            group.MapGet("/getbyid", async (HttpContext httpContext, ITarefaDomain tarefaDomain, string idTarefa) =>
            {
                if (string.IsNullOrWhiteSpace(idTarefa))
                    return Results.BadRequest("id no formato inválido");

                var userId = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                var tarefa = await tarefaDomain.GetByIdAsync(userId, idTarefa);
                return tarefa is null ? Results.NoContent() : Results.Ok(tarefa);
            });

            // POST /api/tarefas/add
            group.MapPost("/add", async (HttpContext httpContext, Tarefa tarefaAddVM, ITarefaDomain tarefaDomain) =>
            {
                // Em Minimal APIs a validação automática não ocorre; valide conforme necessário.
                if (tarefaAddVM == null)
                    return Results.BadRequest("Dados inválidos.");

                var userId = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                var tarefa = new Tarefa
                {
                    Data = tarefaAddVM.Data,
                    Descricao = tarefaAddVM.Descricao,
                    StatusTarefa = tarefaAddVM.StatusTarefa,
                    UserId = userId
                };

                if (await tarefaDomain.AddTarefa(userId, tarefa))
                    return Results.Ok(tarefa);
                else
                    return Results.BadRequest("Erro ao adicionar tarefa");
            });

            return endpoints;
        }
    }
}
