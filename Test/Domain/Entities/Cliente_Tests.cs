using Domain.Entities;
using Xunit;

namespace Test.Domain.Entities;

public class Cliente_Tests
{
    public static IEnumerable<object?[]> ClienteConstructorTestParameters()
    {
        var endereco = new Endereco("Rua A", "123", "Cidade", "Estado", "00000-000");
        yield return new object?[] { null, "João", "joao@email.com", endereco, "123456789", null };
        yield return new object?[] { Guid.NewGuid(), "João", "joao@email.com", endereco, "123456789", null };
        yield return new object?[] { Guid.NewGuid(), "Maria", "maria@email.com", endereco, null, null };
        yield return new object?[] { Guid.Empty, "João", "joao@email.com", endereco, "123456789", typeof(ArgumentException) };
        yield return new object?[] { Guid.NewGuid(), null, "email@email.com", endereco, null, typeof(ArgumentException) };
        yield return new object?[] { Guid.NewGuid(), "", "email@email.com", endereco, null, typeof(ArgumentException) };
        yield return new object?[] { Guid.NewGuid(), "   ", "email@email.com", endereco, null, typeof(ArgumentException) };
        yield return new object?[] { Guid.NewGuid(), "Nome", null, endereco, null, typeof(ArgumentException) };
        yield return new object?[] { Guid.NewGuid(), "Nome", "", endereco, null, typeof(ArgumentException) };
        yield return new object?[] { Guid.NewGuid(), "Nome", "   ", endereco, null, typeof(ArgumentException) };
        yield return new object?[] { Guid.NewGuid(), "Nome", "email@email.com", null, null, typeof(ArgumentNullException) };
    }

    [Theory]
    [MemberData(nameof(ClienteConstructorTestParameters))]
    public void Cliente_Constructor_Test(Guid? id, string nome, string email, Endereco endereco, string? telefone, Type? expectedExceptionType)
    {
        if (expectedExceptionType is null)
        {
            var cliente = new Cliente(id ?? Guid.NewGuid(), nome, email, endereco, telefone);
            Assert.Equal(nome, cliente.Nome);
            Assert.Equal(email, cliente.Email);
            Assert.Equal(endereco, cliente.Endereco);
            Assert.Equal(telefone, cliente.Telefone);
            Assert.True(cliente.Id != Guid.Empty);
        }
        else
        {
            var ex = Assert.Throws(expectedExceptionType, () => new Cliente(id ?? Guid.NewGuid(), nome, email, endereco, telefone));
            Assert.NotNull(ex.Message);
        }
    }
}
