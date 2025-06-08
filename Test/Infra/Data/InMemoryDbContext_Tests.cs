using Domain.Entities;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Test.Infra.Data;

public class InMemoryDbContext_Tests
{
    private InMemoryDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<InMemoryDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new InMemoryDbContext(options);
    }

    [Fact]
    public void AdicionaEBusca_Cliente_Test()
    {
        var dbName = Guid.NewGuid().ToString();
        using var context = CreateContext(dbName);
        var cliente = new Cliente(Guid.NewGuid(), "Nome", "email@email.com", new Endereco("Rua", "1", "Cidade", "Estado", "00000-000"), "123");

        context.Clientes.Add(cliente);
        context.SaveChanges();

        var found = context.Clientes.FirstOrDefault(c => c.Email == "email@email.com");

        Assert.NotNull(found);
        Assert.Equal(cliente.Email, found!.Email);
    }
}
