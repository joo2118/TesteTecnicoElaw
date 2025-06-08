using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(IClienteRepository repository, IMapper mapper, ILogger<ClienteService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<ClienteDTO>> ListarAsync()
        {
            _logger.LogInformation("Listando todos os clientes.");
            var clientes = await _repository.ObterTodosAsync();
            return _mapper.Map<List<ClienteDTO>>(clientes);
        }

        public async Task<ClienteDTO?> ObterAsync(Guid id)
        {
            _logger.LogInformation("Obtendo cliente com ID {Id}", id);
            var cliente = await _repository.ObterPorIdAsync(id);
            return cliente == null ? null : _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<Guid> CriarAsync(ClienteDTO dto)
        {
            _logger.LogInformation("Criando novo cliente com email {Email}", dto.Email);
            var existente = await _repository.ObterPorEmailAsync(dto.Email);
            if (existente != null)
            {
                _logger.LogWarning("Tentativa de criar cliente com email já cadastrado: {Email}", dto.Email);
                throw new InvalidOperationException("Email já cadastrado.");
            }

            var cliente = _mapper.Map<Cliente>(dto);

            await _repository.AdicionarAsync(cliente);
            _logger.LogInformation("Cliente criado com sucesso. ID: {Id}", cliente.Id);
            return cliente.Id;
        }

        public async Task AtualizarAsync(Guid id, ClienteDTO dto)
        {
            _logger.LogInformation("Atualizando cliente com ID {Id}", id);
            var cliente = await _repository.ObterPorIdAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning("Tentativa de atualizar cliente não encontrado. ID: {Id}", id);
                throw new KeyNotFoundException("Cliente não encontrado.");
            }

            _mapper.Map(dto, cliente);
            await _repository.AtualizarAsync(cliente);
            _logger.LogInformation("Cliente atualizado com sucesso. ID: {Id}", id);
        }

        public async Task RemoverAsync(Guid id)
        {
            _logger.LogInformation("Removendo cliente com ID {Id}", id);
            await _repository.RemoverAsync(id);
            _logger.LogInformation("Cliente removido com sucesso. ID: {Id}", id);
        }
    }
}