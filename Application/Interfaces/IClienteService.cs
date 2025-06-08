using Application.DTOs;

namespace Application.Interfaces
{
    public interface IClienteService
    {
        Task<List<ClienteDTO>> ListarAsync();
        Task<ClienteDTO?> ObterAsync(Guid id);
        Task<Guid> CriarAsync(ClienteDTO dto);
        Task AtualizarAsync(Guid id, ClienteDTO dto);
        Task RemoverAsync(Guid id);
    }
}
