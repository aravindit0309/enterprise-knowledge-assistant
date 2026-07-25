using EnterpriseKnowledgeAssistant.Application.Features.Documents.Commands.ProcessDocument;
using EnterpriseKnowledgeAssistant.Application.Features.Documents.Commands.UploadDocument;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseKnowledgeAssistant.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<UploadDocumentResponse>> Upload(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required.");
            }

            await using var stream = file.OpenReadStream();

            var response = await _mediator.Send(
                new UploadDocumentCommand(
                    stream,
                    file.FileName,
                    file.ContentType,
                    file.Length),
                cancellationToken);

            return Ok(response);
        }

        [HttpPost("{documentId:guid}/process")]
        public async Task<ActionResult<ProcessDocumentResponse>> ProcessDocument(Guid documentId, CancellationToken cancellationToken)
        {
            var command = new ProcessDocumentCommand(documentId);

            var response = await _mediator.Send( command, cancellationToken);

            return Ok(response);
        }
    }
}
