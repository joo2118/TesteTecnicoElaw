using Domain.Entities;
using Domain.Interfaces;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infra.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly InMemoryDbContext _context;
    private readonly ILogger<ClienteRepository> _logger;

    public ClienteRepository(InMemoryDbContext context, ILogger<ClienteRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<Cliente>> ObterTodosAsync()
    {
        _logger.LogInformation("Obtendo todos os clientes do banco de dados.");
        return await _context.Clientes.ToListAsync();
    }

    public async Task<Cliente?> ObterPorIdAsync(Guid id)
    {
        _logger.LogInformation("Obtendo cliente por ID: {Id} do banco de dados.", id);
        return await _context.Clientes.FindAsync(id);
    }

    public async Task<Cliente?> ObterPorEmailAsync(string email) =>
        await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);

    public async Task AdicionarAsync(Cliente cliente)
    {
        _logger.LogInformation("Adicionando novo cliente ao banco de dados. ID: {Id}", cliente.Id);
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Cliente cliente)
    {
        _logger.LogInformation("Atualizando cliente no banco de dados. ID: {Id}", cliente.Id);
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Guid id)
    {
        var cliente = await ObterPorIdAsync(id);
        if (cliente != null)
        {
            _logger.LogInformation("Removendo cliente do banco de dados. ID: {Id}", id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
        else
        {
            _logger.LogWarning("Tentativa de remover cliente não encontrado. ID: {Id}", id);
        }
    }
}
