using Application.DTOs;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Test.Application.Services;

public class ClienteService_Tests
{
    public static IEnumerable<object?[]> ClienteServiceConstructorTestParameters()
    {
        var repo = Substitute.For<IClienteRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<ClienteService>>();
        yield return new object?[] { repo, mapper, logger, null };
        yield return new object?[] { null, mapper, logger, typeof(ArgumentNullException) };
        yield return new object?[] { repo, null, logger, typeof(ArgumentNullException) };
        yield return new object?[] { repo, mapper, null, typeof(ArgumentNullException) };
    }

    [Theory]
    [MemberData(nameof(ClienteServiceConstructorTestParameters))]
    public void ClienteService_Constructor_Test(IClienteRepository repo, IMapper mapper, ILogger<ClienteService> logger, Type? expectedExceptionType)
    {
        if (expectedExceptionType is null)
        {
            var service = new ClienteService(repo, mapper, logger);
            Assert.NotNull(service);
        }
        else
        {
            var ex = Assert.Throws(expectedExceptionType, () => new ClienteService(repo, mapper, logger));
            Assert.NotNull(ex.Message);
        }
    }

    [Fact]
    public async Task ListarAsync_Sucesso_Test()
    {
        var repo = Substitute.For<IClienteRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<ClienteService>>();
        var endereco = new Endereco("Rua A", "123", "Cidade", "Estado", "00000-000");
        var enderecoDTO = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");
        var clientes = new List<Cliente> { new Cliente(Guid.NewGuid(), "Nome", "email@email.com", endereco, null) };
        var clientesDto = new List<ClienteDTO> { new ClienteDTO(Guid.NewGuid(), "Nome", "email@email.com", enderecoDTO, null) };

        repo.ObterTodosAsync().Returns(clientes);
        mapper.Map<List<ClienteDTO>>(clientes).Returns(clientesDto);

        var service = new ClienteService(repo, mapper, logger);
        var result = await service.ListarAsync();

        Assert.Equal(clientesDto, result);
    }

    [Fact]
    public async Task ListarAsync_Erro_Test()
    {
        var repo = Substitute.For<IClienteRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<ClienteService>>();
        var endereco = new Endereco("Rua A", "123", "Cidade", "Estado", "00000-000");
        var enderecoDTO = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");
        var clientes = new List<Cliente> { new Cliente(Guid.NewGuid(), "Nome", "email@email.com", endereco, null) };
        var clientesDto = new List<ClienteDTO> { new ClienteDTO(Guid.NewGuid(), "Nome", "email@email.com", enderecoDTO, null) };
        var exception = new InvalidOperationException("Erro ao listar clientes");

        repo.ObterTodosAsync().Throws(exception);

        var actual = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            var service = new ClienteService(repo, mapper, logger);
            await service.ListarAsync();
        });

        Assert.Equal(exception.Message, actual.Message);
    }

    [Fact]
    public async Task ObterAsync_Sucesso_Test()
    {
        var repo = Substitute.For<IClienteRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<ClienteService>>();
        var endereco = new Endereco("Rua A", "123", "Cidade", "Estado", "00000-000");
        var enderecoDTO = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");
        var cliente = new Cliente(Guid.NewGuid(), "Nome", "email@email.com", endereco, null);
        var clienteDto = new ClienteDTO(Guid.NewGuid(), "Nome", "email@email.com", enderecoDTO, null);

        repo.ObterPorIdAsync(cliente.Id).Returns(cliente);
        mapper.Map<ClienteDTO>(cliente).Returns(clienteDto);

        var service = new ClienteService(repo, mapper, logger);
        var result = await service.ObterAsync(cliente.Id);

        Assert.Equal(clienteDto, result);
    }

    [Fact]
    public async Task ObterAsync_NaoEncontrado_Test()
    {
        var repo = Substitute.For<IClienteRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<ClienteService>>();

        repo.ObterPorIdAsync(Arg.Any<Guid>()).Returns((Cliente?)null);

        var service = new ClienteService(repo, mapper, logger);
        var result = await service.ObterAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task ObterAsync_Erro_Test()
    {
        var repo = Substitute.For<IClienteRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<ClienteService>>();
        var endereco = new Endereco("Rua A", "123", "Cidade", "Estado", "00000-000");
        var enderecoDTO = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");
        var clientes = new List<Cliente> { new Cliente(Guid.NewGuid(), "Nome", "email@email.com", endereco, null) };
        var clientesDto = new List<ClienteDTO> { new ClienteDTO(Guid.NewGuid(), "Nome", "email@email.com", enderecoDTO, null) };
        var exception = new InvalidOperationException("Erro ao obter clientes");

        repo.ObterPorIdAsync(Arg.Any<Guid>()).Throws(exception);

        var actual = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            var service = new ClienteService(repo, mapper, logger);
            await service.ObterAsync(Guid.NewGuid());
        });

        Assert.Equal(exception.Message, actual.Message);
    }

    [Fact]
    public async Task CriarAsync_Erro_EmailExistente_Test()
    {
        var repo = Substitute.For<IClienteRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<ClienteService>>();
        var endereco = new Endereco("Rua A", "123", "Cidade", "Estado", "00000-000");
        var enderecoDTO = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");
        var dto = new ClienteDTO(Guid.NewGuid(), "Nome", "email@email.com", enderecoDTO, null);

        repo.ObterPorEmailAsync(dto.Email).Returns(new Cliente(Guid.NewGuid(), "Nome", "email@email.com", endereco, null));

        var service = new ClienteService(repo, mapper, logger);
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CriarAsync(dto));
    }

    [Fact]
    public async Task CriarAsync_Sucesso_Test()
    {
        var repo = Substitute.For<IClienteRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<ClienteService>>();
        var endereco = new Endereco("Rua A", "123", "Cidade", "Estado", "00000-000");
        var enderecoDTO = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");
        var cliente = new Cliente(Guid.NewGuid(), "Nome", "email@email.com", endereco, null);
        var dto = new ClienteDTO(cliente.Id, cliente.Nome!, cliente.Email!, enderecoDTO, cliente.Telefone);

        repo.ObterPorEmailAsync(dto.Email).Returns((Cliente?)null);
        mapper.Map<Cliente>(dto).Returns(cliente);

        var service = new ClienteService(repo, mapper, logger);
        var result = await service.CriarAsync(dto);

        await repo.Received(1).AdicionarAsync(cliente);
        Assert.Equal(cliente.Id, result);
    }

    [Fact]
    public async Task AtualizarAsync_NotFound_Tests()
    {
        var repo = Substitute.For<IClienteRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<ClienteService>>();
        var enderecoDTO = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");

        repo.ObterPorIdAsync(Arg.Any<Guid>()).Returns((Cliente?)null);

        var service = new ClienteService(repo, mapper, logger);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.AtualizarAsync(Guid.NewGuid(), new ClienteDTO(Guid.NewGuid(), "Nome", "email@email.com", enderecoDTO, null)));
    }

    [Fact]
    public async Task AtualizarAsync_Sucesso_Test()
    {
        var repo = Substitute.For<IClienteRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<ClienteService>>();
        var endereco = new Endereco("Rua A", "123", "Cidade", "Estado", "00000-000");
        var enderecoDTO = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");
        var cliente = new Cliente(Guid.NewGuid(), "Nome", "email@email.com", endereco, null);

        repo.ObterPorIdAsync(cliente.Id).Returns(cliente);

        var dto = new ClienteDTO(cliente.Id, "Nome", "email@email.com", enderecoDTO, null);
        var service = new ClienteService(repo, mapper, logger);

        await service.AtualizarAsync(cliente.Id, dto);
        mapper.Received(1).Map(dto, cliente);
        await repo.Received(1).AtualizarAsync(cliente);
    }

    [Fact]
    public async Task AtualizarAsync_Erro_Test()
    {
        var repo = Substitute.For<IClienteRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<ClienteService>>();
        var endereco = new Endereco("Rua A", "123", "Cidade", "Estado", "00000-000");
        var enderecoDTO = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");
        var cliente = new Cliente(Guid.NewGuid(), "Nome", "email@email.com", endereco, null);
        var dto = new ClienteDTO(cliente.Id, "Nome", "email@email.com", enderecoDTO, null);
        var exception = new InvalidOperationException("Erro ao atualizar clientes");

        repo.ObterPorIdAsync(Arg.Any<Guid>()).Throws(exception);

        var actual = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            var service = new ClienteService(repo, mapper, logger);
            await service.AtualizarAsync(cliente.Id, dto);
        });

        Assert.Equal(exception.Message, actual.Message);
    }

    [Fact]
    public async Task RemoverAsync_Sucesso_Test()
    {
        var repo = Substitute.For<IClienteRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<ClienteService>>();
        var service = new ClienteService(repo, mapper, logger);
        var id = Guid.NewGuid();

        await service.RemoverAsync(id);
        await repo.Received(1).RemoverAsync(id);
    }
}
