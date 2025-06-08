using Application.DTOs;
using Xunit;

namespace Test.Application.DTOs;

public class ClienteDTO_Tests
{
    public static IEnumerable<object?[]> ClienteDTOConstructorTestParameters()
    {
        var endereco = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");
        yield return new object?[] { Guid.NewGuid(), "João", "joao@email.com", endereco, "123456789", null };
        yield return new object?[] { null, "Maria", "maria@email.com", endereco, null, null };
        yield return new object?[] { Guid.Empty, "Maria", "maria@email.com", endereco, null, null };
        yield return new object?[] { Guid.NewGuid(), null, "email@email.com", endereco, null, typeof(ArgumentException) };
        yield return new object?[] { Guid.NewGuid(), "", "email@email.com", endereco, null, typeof(ArgumentException) };
        yield return new object?[] { Guid.NewGuid(), "   ", "email@email.com", endereco, null, typeof(ArgumentException) };
        yield return new object?[] { Guid.NewGuid(), "Nome", null, endereco, null, typeof(ArgumentException) };
        yield return new object?[] { Guid.NewGuid(), "Nome", "", endereco, null, typeof(ArgumentException) };
        yield return new object?[] { Guid.NewGuid(), "Nome", "   ", endereco, null, typeof(ArgumentException) };
        yield return new object?[] { Guid.NewGuid(), "Nome", "email@email.com", null, null, typeof(ArgumentNullException) };
    }

    [Theory]
    [MemberData(nameof(ClienteDTOConstructorTestParameters))]
    public void ClienteDTO_Constructor_Test(Guid? id, string nome, string email, EnderecoDTO endereco, string? telefone, Type? expectedExceptionType)
    {
        if (expectedExceptionType is null)
        {
            var dto = new ClienteDTO(id, nome, email, endereco, telefone);
            Assert.Equal(nome, dto.Nome);
            Assert.Equal(email, dto.Email);
            Assert.Equal(endereco, dto.Endereco);
            Assert.Equal(telefone, dto.Telefone);
            Assert.True(dto.Id.HasValue && dto.Id != Guid.Empty);
        }
        else
        {
            var ex = Assert.Throws(expectedExceptionType, () => new ClienteDTO(id, nome, email, endereco, telefone));
            Assert.NotNull(ex.Message);
        }
    }
}
