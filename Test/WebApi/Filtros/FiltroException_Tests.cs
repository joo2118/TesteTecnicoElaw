using Infra.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using NSubstitute;
using Xunit;

namespace Test.WebApi.Filtros
{
    public class FIltroException_Tests
    {
        public static IEnumerable<object?[]> Constructor_Parameters()
        {
            yield return new object?[] { null, Substitute.For<ILogger<FIltroException>>(), "hostEnvironment" };
            yield return new object?[] { Substitute.For<IHostEnvironment>(), null, "logger" };
        }

        [Theory]
        [MemberData(nameof(Constructor_Parameters))]
        public void Constructor_Test(IHostEnvironment hostEnvironment, ILogger<FIltroException> logger, string paramName)
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new FIltroException(hostEnvironment!, logger!));
            Assert.Equal(paramName, ex.ParamName);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void OnException_Retorna_InternalServerError_Test(bool isDevelopment)
        {
            var requestId = Guid.NewGuid();
            var route = "/test-route";
            var exception = new InvalidOperationException("Test message");

            var hostEnvironment = Substitute.For<IHostEnvironment>();
            hostEnvironment.EnvironmentName.Returns(isDevelopment ? "Development" : "Production");

            var logger = Substitute.For<ILogger<FIltroException>>();

            var httpContext = Substitute.For<HttpContext>();
            httpContext.TraceIdentifier.Returns(requestId.ToString());
            httpContext.Request.Path.Returns(new PathString(route));

            var context = new ExceptionContext(
                new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>())
            {
                Exception = exception
            };

            var filter = new FIltroException(hostEnvironment, logger);

            filter.OnException(context);

            var result = context.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, result!.StatusCode);

            if (isDevelopment)
            {
                var expectedMessage = $"Solicitação '{requestId}' para a rota '{route}' terminou com Erro: [500] {exception.Message}";
                Assert.Equal(expectedMessage, result.Value);
            }
            else
            {
                var expectedMessage = $"Solicitação '{requestId}' terminou com Erro. Verifique os logs para mais detalhes.";
                Assert.Equal(expectedMessage, result.Value);
            }
        }
    }
}
