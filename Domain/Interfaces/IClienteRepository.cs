using Domain.Entities;

namespace Domain.Interfaces
{

    public interface IClienteRepository
    {
        Task<List<Cliente>> ObterTodosAsync();
        Task<Cliente?> ObterPorIdAsync(Guid id);
        Task<Cliente?> ObterPorEmailAsync(string email);
        Task AdicionarAsync(Cliente cliente);
        Task AtualizarAsync(Cliente cliente);
        Task RemoverAsync(Guid id);
    }
}