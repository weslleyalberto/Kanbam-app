using Kabam.api.Data;
using Kabam.api.VM;
using Kabam.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kabam.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TarefasController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly UserManager<User> _userM;

        public TarefasController(UserContext context, UserManager<User> userM)
        {
            _context = context;
            _userM = userM;
        }
        
        [HttpGet("getall")]
        public async Task<ActionResult> GetAll()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = await _context.Tarefas.Where(c => c.UserId == userId).OrderBy(c=> c.Data).ToListAsync();
            if (result is null)
                return NoContent();
            else
                return Ok(result);
        }
        [HttpGet("getbyid")]
        public async Task<ActionResult> GetByIdAsync(string idTarefa)
        {
            if (string.IsNullOrWhiteSpace(idTarefa))
            {

                return BadRequest("id no formato inválido");
            }
            else
            {
                var tarefa = await _context.Tarefas.FirstOrDefaultAsync(c => c.Id.ToString() == idTarefa);
                if (tarefa is null)
                {
                    return NoContent();
                }
                return Ok(tarefa);
            }
        }
        [HttpPost("add")]
       
        public async Task<ActionResult> AddTarefa([FromBody]TarefaAddVM tarefaAddVM)
        {
           
          
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
            {
               
                Tarefa tarefa = new()
                {
                    Data = DateTime.Parse(tarefaAddVM.Data),
                    Descricao = tarefaAddVM.Descricao,
                    StatusTarefa = tarefaAddVM.StatusTarefa,
                    UserId = User.FindFirst(ClaimTypes.Name)?.Value
                };
                await _context.Tarefas.AddAsync(tarefa);
                await _context.SaveChangesAsync();
                return Ok(tarefa);
            }
        }
    }
}
