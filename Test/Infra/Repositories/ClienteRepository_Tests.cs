using Domain.Entities;
using Infra.Data;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace Test.Infra.Repositories;

public class ClienteRepository_Tests
{
    public static IEnumerable<object?[]> ClienteRepositoryConstructorTestParameters()
    {
        var context = Substitute.For<InMemoryDbContext>(new DbContextOptionsBuilder<InMemoryDbContext>().Options);
        var logger = Substitute.For<ILogger<ClienteRepository>>();
        yield return new object?[] { context, logger, null };
        yield return new object?[] { null, logger, typeof(ArgumentNullException) };
        yield return new object?[] { context, null, typeof(ArgumentNullException) };
    }

    [Theory]
    [MemberData(nameof(ClienteRepositoryConstructorTestParameters))]
    public void ClienteRepository_Constructor_Test(InMemoryDbContext context, ILogger<ClienteRepository> logger, Type? expectedExceptionType)
    {
        if (expectedExceptionType is null)
        {
            var repo = new ClienteRepository(context, logger);
            Assert.NotNull(repo);
        }
        else
        {
            var ex = Assert.Throws(expectedExceptionType, () => new ClienteRepository(context, logger));
            Assert.NotNull(ex.Message);
        }
    }

    [Fact]
    public async Task ObterTodosAsync_Sucesso_Test()
    {
        var options = new DbContextOptionsBuilder<InMemoryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        using var context = new InMemoryDbContext(options);
        var logger = Substitute.For<ILogger<ClienteRepository>>();

        context.Clientes.Add(new Cliente(Guid.NewGuid(), "Nome", "email@email.com",
            new Endereco("Rua", "1", "Cidade", "Estado", "00000-000"), null));
        context.SaveChanges();

        var repo = new ClienteRepository(context, logger);

        var result = await repo.ObterTodosAsync();

        Assert.Single(result);
        Assert.Equal("Nome", result[0].Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_Sucesso_Test()
    {
        var context = Substitute.For<InMemoryDbContext>(new DbContextOptionsBuilder<InMemoryDbContext>().Options);
        var logger = Substitute.For<ILogger<ClienteRepository>>();

        var repo = new ClienteRepository(context, logger);

        var id = Guid.NewGuid();
        await repo.ObterPorIdAsync(id);
        await context.Clientes.Received().FindAsync(id);
    }

    [Fact]
    public async Task ObterPorEmailAsync_Sucesso_Test()
    {
        var options = new DbContextOptionsBuilder<InMemoryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        using var context = new InMemoryDbContext(options);
        var logger = Substitute.For<ILogger<ClienteRepository>>();

        var email = "test@email.com";
        context.Clientes.Add(new Cliente(Guid.NewGuid(), "Nome", email,
            new Endereco("Rua", "1", "Cidade", "Estado", "00000-000"), null));
        context.SaveChanges();

        var repo = new ClienteRepository(context, logger);
        var result = await repo.ObterPorEmailAsync(email);

        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task AdicionarAsync_Sucesso_Test()
    {
        var context = Substitute.For<InMemoryDbContext>(new DbContextOptionsBuilder<InMemoryDbContext>().Options);
        var logger = Substitute.For<ILogger<ClienteRepository>>();

        var repo = new ClienteRepository(context, logger);
        var cliente = Substitute.For<Cliente>(Guid.NewGuid(), "Nome", "email@email.com", Substitute.For<Endereco>(), null);

        await repo.AdicionarAsync(cliente);
        context.Clientes.Received().Add(cliente);
        await context.Received().SaveChangesAsync();
    }

    [Fact]
    public async Task AtualizarAsync_Sucesso_Test()
    {
        var context = Substitute.For<InMemoryDbContext>(new DbContextOptionsBuilder<InMemoryDbContext>().Options);
        var logger = Substitute.For<ILogger<ClienteRepository>>();

        var repo = new ClienteRepository(context, logger);
        var cliente = Substitute.For<Cliente>(Guid.NewGuid(), "Nome", "email@email.com", Substitute.For<Endereco>(), null);

        await repo.AtualizarAsync(cliente);
        context.Clientes.Received().Update(cliente);
        await context.Received().SaveChangesAsync();
    }

    [Fact]
    public async Task RemoverAsync_Sucesso_Test()
    {
        var context = Substitute.For<InMemoryDbContext>(new DbContextOptionsBuilder<InMemoryDbContext>().Options);
        var logger = Substitute.For<ILogger<ClienteRepository>>();
        var repo = new ClienteRepository(context, logger);
        var id = Guid.NewGuid();
        var cliente = Substitute.For<Cliente>(id, "Nome", "email@email.com", Substitute.For<Endereco>(), null);

        context.Clientes.FindAsync(id).Returns(cliente);

        await repo.RemoverAsync(id);
        context.Clientes.Received().Remove(cliente);
        await context.Received().SaveChangesAsync();
    }

    [Fact]
    public async Task RemoverAsync_NotFound_Test()
    {
        var context = Substitute.For<InMemoryDbContext>(new DbContextOptionsBuilder<InMemoryDbContext>().Options);
        var logger = Substitute.For<ILogger<ClienteRepository>>();
        var repo = new ClienteRepository(context, logger);
        var id = Guid.NewGuid();

        context.Clientes.FindAsync(id).Returns((Cliente?)null);

        await repo.RemoverAsync(id);
        context.Clientes.DidNotReceive().Remove(Arg.Any<Cliente>());
        await context.Received(0).SaveChangesAsync();
    }
}
