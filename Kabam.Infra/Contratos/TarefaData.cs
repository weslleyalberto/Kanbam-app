using Kabam.Domain;
using Kabam.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kabam.Infra.Contratos
{
    public class TarefaData : ITarefaDomain
    {
        private readonly DbContextOptions<UserContext> _options;
        public TarefaData()
        {
            _options = new DbContextOptions<UserContext>();
        }
        public async Task<bool> AddTarefa(string idUser,Tarefa tarefaAddVM)
        {
            using (var context = new UserContext(_options))
            {
                tarefaAddVM.UserId = idUser;
                await context.Tarefas.AddAsync(tarefaAddVM);
                return await context.SaveChangesAsync() > 0;

            }
        }

        public Task<List<Tarefa>> GetAll(string idUser)
        {
            using (var context = new UserContext(_options))
            {
                return context.Tarefas.Where(t=> t.UserId == idUser).ToListAsync();
            }
        }

        public Task<Tarefa> GetByIdAsync(string idUser,string idTarefa)
        {
            using (var context = new UserContext(_options))
            {
                return context.Tarefas.Where(t=> t.UserId == idUser && t.Id.ToString() == idTarefa).FirstOrDefaultAsync();
            }
        }
    }
}
