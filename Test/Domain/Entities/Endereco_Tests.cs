using Domain.Entities;
using Xunit;

namespace Test.Domain.Entities;

public class Endereco_Tests
{
    public static IEnumerable<object?[]> EnderecoConstructorTestParameters()
    {
        yield return new object?[] { "Rua A", "123", "Cidade", "Estado", "00000-000", null };
        yield return new object?[] { null, "123", "Cidade", "Estado", "00000-000", typeof(ArgumentException) };
        yield return new object?[] { "", "123", "Cidade", "Estado", "00000-000", typeof(ArgumentException) };
        yield return new object?[] { "   ", "123", "Cidade", "Estado", "00000-000", typeof(ArgumentException) };
        yield return new object?[] { "Rua A", null, "Cidade", "Estado", "00000-000", typeof(ArgumentException) };
        yield return new object?[] { "Rua A", "", "Cidade", "Estado", "00000-000", typeof(ArgumentException) };
        yield return new object?[] { "Rua A", "   ", "Cidade", "Estado", "00000-000", typeof(ArgumentException) };
        yield return new object?[] { "Rua A", "123", null, "Estado", "00000-000", typeof(ArgumentException) };
        yield return new object?[] { "Rua A", "123", "", "Estado", "00000-000", typeof(ArgumentException) };
        yield return new object?[] { "Rua A", "123", "   ", "Estado", "00000-000", typeof(ArgumentException) };
        yield return new object?[] { "Rua A", "123", "Cidade", null, "00000-000", typeof(ArgumentException) };
        yield return new object?[] { "Rua A", "123", "Cidade", "", "00000-000", typeof(ArgumentException) };
        yield return new object?[] { "Rua A", "123", "Cidade", "   ", "00000-000", typeof(ArgumentException) };
        yield return new object?[] { "Rua A", "123", "Cidade", "Estado", null, typeof(ArgumentException) };
        yield return new object?[] { "Rua A", "123", "Cidade", "Estado", "", typeof(ArgumentException) };
        yield return new object?[] { "Rua A", "123", "Cidade", "Estado", "   ", typeof(ArgumentException) };
    }

    [Theory]
    [MemberData(nameof(EnderecoConstructorTestParameters))]
    public void Endereco_Constructor_Test(string rua, string numero, string cidade, string estado, string cep, Type? expectedExceptionType)
    {
        if (expectedExceptionType is null)
        {
            var endereco = new Endereco(rua, numero, cidade, estado, cep);
            Assert.Equal(rua, endereco.Rua);
            Assert.Equal(numero, endereco.Numero);
            Assert.Equal(cidade, endereco.Cidade);
            Assert.Equal(estado, endereco.Estado);
            Assert.Equal(cep, endereco.CEP);
        }
        else
        {
            var ex = Assert.Throws(expectedExceptionType, () => new Endereco(rua, numero, cidade, estado, cep));
            Assert.NotNull(ex.Message);
        }
    }
}
