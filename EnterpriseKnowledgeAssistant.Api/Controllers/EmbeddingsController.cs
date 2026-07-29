using EnterpriseKnowledgeAssistant.Application.Features.Embeddings.GenerateEmbedding;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseKnowledgeAssistant.Api.Controllers
{
    [ApiController]
    [Route("api/embeddings")]
    public class EmbeddingsController : ControllerBase
    {
        private readonly ISender _sender;

        public EmbeddingsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("test")]
        public async Task<ActionResult<GenerateEmbeddingResponse>> Generate([FromBody] GenerateEmbeddingRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _sender.Send(new GenerateEmbeddingCommand(request.Text),cancellationToken);

            return Ok(response);
        }
    }

    public sealed record GenerateEmbeddingRequest(string Text);
}
