using EnterpriseKnowledgeAssistant.Application.Features.Search.SemanticSearch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseKnowledgeAssistant.Api.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly ISender _sender;

        public SearchController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<ActionResult<IReadOnlyList<SemanticSearchResult>>> Search(
            [FromBody] SemanticSearchRequest request,CancellationToken cancellationToken)
        {
            var result = await _sender.Send( new SemanticSearchCommand(request.Query, request.Limit),
                cancellationToken);

            return Ok(result);
        }
    }

    public sealed record SemanticSearchRequest( string Query, int Limit = 5);
}
