using Application.DTOs;
using Application.Interfaces;
using Infra.Filters;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("clientes")]
    [TypeFilter(typeof(FIltroException))]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClientesController(IClienteService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Lista todos os clientes
        /// </summary>
        /// <returns>Uma lista de clientes</returns>
        /// <response code="200">Lista de clientes retornada com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ClienteDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get() => Ok(await _service.ListarAsync());

        /// <summary>
        /// Busca um cliente pelo ID
        /// </summary>
        /// <param name="id">O ID do cliente</param>
        /// <returns>O cliente correspondente ao ID</returns>
        /// <response code="200">Cliente encontrado</response>
        /// <response code="404">Cliente não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClienteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cliente = await _service.ObterAsync(id);
            return cliente == null ? NotFound($"Cliente ID {id} não encontrado") : Ok(cliente);
        }

        /// <summary>
        /// Cria um novo cliente
        /// </summary>
        /// <param name="dto">Os dados do cliente a ser criado</param>
        /// <returns>O ID do cliente criado</returns>
        /// <response code="201">Cliente criado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] ClienteDTO dto)
        {
            var id = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        /// <summary>
        /// Atualiza um cliente existente
        /// </summary>
        /// <param name="id">O ID do cliente a ser atualizado</param>
        /// <param name="dto">Os novos dados do cliente</param>
        /// <returns>Um action result confirmando a solicitação</returns>
        /// <response code="204">Cliente atualizado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="404">Cliente não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, [FromBody] ClienteDTO dto)
        {
            await _service.AtualizarAsync(id, dto);
            return NoContent();
        }

        /// <summary>
        /// Deleta um Cliente
        /// </summary>
        /// <param name="id">O ID do cliente a ser deletado</param>
        /// <returns>Um action result confirmando a solicitação</returns>
        /// <response code="204">Sem conteudo</response>
        /// <remarks>
        /// Esse endpoint permite a exclusão de um cliente existente.
        /// O ID do cliente deve ser fornecido na URL.
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.RemoverAsync(id);
            return NoContent();
        }
    }
}