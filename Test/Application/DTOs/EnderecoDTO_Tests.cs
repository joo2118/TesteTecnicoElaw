using Application.DTOs;
using Xunit;

namespace Test.Application.DTOs;

public class EnderecoDTO_Tests
{
    public static IEnumerable<object?[]> EnderecoDTOConstructorTestParameters()
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
    [MemberData(nameof(EnderecoDTOConstructorTestParameters))]
    public void EnderecoDTO_Constructor_Test(string rua, string numero, string cidade, string estado, string cep, Type? expectedExceptionType)
    {
        if (expectedExceptionType is null)
        {
            var dto = new EnderecoDTO(rua, numero, cidade, estado, cep);
            Assert.Equal(rua, dto.Rua);
            Assert.Equal(numero, dto.Numero);
            Assert.Equal(cidade, dto.Cidade);
            Assert.Equal(estado, dto.Estado);
            Assert.Equal(cep, dto.CEP);
        }
        else
        {
            var ex = Assert.Throws(expectedExceptionType, () => new EnderecoDTO(rua, numero, cidade, estado, cep));
            Assert.NotNull(ex.Message);
        }
    }
}
