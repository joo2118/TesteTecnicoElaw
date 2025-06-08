using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infra.Filters
{
    public class FIltroException : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<FIltroException> _logger;

        public FIltroException(IHostEnvironment hostEnvironment, ILogger<FIltroException> logger)
        {
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void OnException(ExceptionContext context)
        {
            var statusCode = StatusCodes.Status500InternalServerError; ;

            context.Result = CriaObjetoResultado(context.HttpContext, context.Exception, statusCode);
        }

        private ObjectResult CriaObjetoResultado(HttpContext context, Exception e, int statusCode)
        {
            var idSolicitacao = !string.IsNullOrWhiteSpace(context.TraceIdentifier)
                ? context.TraceIdentifier
                : Guid.NewGuid().ToString();

            _logger.LogError(e, "Solicitação '{IdSolicitacao}' para a rota '{Rota}' terminou com Erro: [{StatusCode}] {MensagemErro}",
                idSolicitacao, context.Request.Path, statusCode, e.Message);

            var message = _hostEnvironment.IsDevelopment()
                ? $"Solicitação '{idSolicitacao}' para a rota '{context.Request.Path}' terminou com Erro: [{statusCode}] {e.Message}"
                : $"Solicitação '{idSolicitacao}' terminou com Erro. Verifique os logs para mais detalhes.";

            return new ObjectResult(message)
            {
                StatusCode = statusCode
            };
        }
    }
}
