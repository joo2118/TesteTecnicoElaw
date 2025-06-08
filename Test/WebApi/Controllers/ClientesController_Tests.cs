using Application.DTOs;
using Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WebApi.Controllers;
using Xunit;

namespace Test.WebApi.Controllers
{
    public class ClientesController_Tests
    {
        public static IEnumerable<object?[]> ConstructorParameters()
        {
            yield return new object?[] { null, "service" };
        }

        [Theory]
        [MemberData(nameof(ConstructorParameters))]
        public void Constructor_Test(IClienteService service, string expectedParamName)
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ClientesController(service!));
            Assert.Equal(expectedParamName, ex.ParamName);
        }

        [Fact]
        public async Task Get_Sucesso_Test()
        {
            var mockService = Substitute.For<IClienteService>();
            var endereco = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");
            var cliente = new ClienteDTO(Guid.NewGuid(), "João", "joao@email.com", endereco, "123456789");
            var expectedList = new List<ClienteDTO> { cliente };

            mockService.ListarAsync().Returns(expectedList);
            var controller = new ClientesController(mockService);

            var result = await controller.Get();

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public async Task GetById_Sucesso_Test()
        {
            var mockService = Substitute.For<IClienteService>();
            var id = Guid.NewGuid();
            var endereco = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");
            var cliente = new ClienteDTO(Guid.NewGuid(), "João", "joao@email.com", endereco, "123456789");

            mockService.ObterAsync(id).Returns(cliente);
            var controller = new ClientesController(mockService);

            var result = await controller.GetById(id);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(cliente);
        }

        [Fact]
        public async Task GetById_Should_Return_NotFound_When_Client_Does_Not_Exist()
        {
            var mockService = Substitute.For<IClienteService>();
            var id = Guid.NewGuid();

            mockService.ObterAsync(id).Returns((ClienteDTO?)null);
            var controller = new ClientesController(mockService);

            var result = await controller.GetById(id);

            var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be($"Cliente ID {id} não encontrado");
        }

        [Fact]
        public async Task Post_Sucesso_Test()
        {
            var mockService = Substitute.For<IClienteService>();
            var endereco = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");
            var cliente = new ClienteDTO(Guid.NewGuid(), "João", "joao@email.com", endereco, "123456789");
            var newId = Guid.NewGuid();

            mockService.CriarAsync(cliente).Returns(newId);
            var controller = new ClientesController(mockService);

            var result = await controller.Post(cliente);

            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(controller.GetById));
            createdResult.RouteValues!["id"].Should().Be(newId);
        }

        [Fact]
        public async Task Post_InternalServerError_Test()
        {
            var mockService = Substitute.For<IClienteService>();
            var endereco = new EnderecoDTO("Rua A", "1", "Cidade", "Estado", "00000-000");
            var cliente = new ClienteDTO(null, "Nome", "email@email.com", endereco);
            mockService.CriarAsync(cliente).Returns(Task.FromException<Guid>(new Exception("Erro interno")));
            var controller = new ClientesController(mockService);

            Func<Task> act = async () => await controller.Post(cliente);
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro interno");
        }

        [Fact]
        public async Task Put_Sucesso_Test()
        {
            var mockService = Substitute.For<IClienteService>();
            var id = Guid.NewGuid();
            var endereco = new EnderecoDTO("Rua A", "123", "Cidade", "Estado", "00000-000");
            var cliente = new ClienteDTO(Guid.NewGuid(), "João", "joao@email.com", endereco, "123456789");

            var controller = new ClientesController(mockService);

            var result = await controller.Put(id, cliente);

            result.Should().BeOfType<NoContentResult>();
            await mockService.Received(1).AtualizarAsync(id, cliente);
        }

        [Fact]
        public async Task Put_nternalServerError_Test()
        {
            var mockService = Substitute.For<IClienteService>();
            var endereco = new EnderecoDTO("Rua A", "1", "Cidade", "Estado", "00000-000");
            var id = Guid.NewGuid();
            var cliente = new ClienteDTO(id, "Nome", "email@email.com", endereco);
            mockService.AtualizarAsync(id, cliente).Returns(Task.FromException(new Exception("Erro interno")));
            var controller = new ClientesController(mockService);

            Func<Task> act = async () => await controller.Put(id, cliente);
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro interno");
        }

        [Fact]
        public async Task Delete_Sucesso_Test()
        {
            var mockService = Substitute.For<IClienteService>();
            var id = Guid.NewGuid();

            var controller = new ClientesController(mockService);

            var result = await controller.Delete(id);

            result.Should().BeOfType<NoContentResult>();
            await mockService.Received(1).RemoverAsync(id);
        }

        [Fact]
        public async Task Delete_InternalServerError_Test()
        {
            var mockService = Substitute.For<IClienteService>();
            var id = Guid.NewGuid();
            mockService.RemoverAsync(id).Returns(Task.FromException(new Exception("Erro interno")));
            var controller = new ClientesController(mockService);

            Func<Task> act = async () => await controller.Delete(id);
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro interno");
        }
    }
}
