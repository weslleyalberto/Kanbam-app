namespace Kabam.Domain.Interfaces
{
    public interface ITarefaDomain
    {
        Task<List<Tarefa>> GetAll(string idUser);
        Task<Tarefa> GetByIdAsync(string idUser,string idTarefa);

        Task<bool> AddTarefa(string idUser,Tarefa tarefaAddVM);
    }
}
