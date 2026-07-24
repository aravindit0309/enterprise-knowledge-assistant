using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using EnterpriseKnowledgeAssistant.Application.Features.Chat.Commands.SendMessage;
using EnterpriseKnowledgeAssistant.Application.Features.Chat.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseKnowledgeAssistant.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly SendMessageCommandHandler _handler;

        public ChatController(SendMessageCommandHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> Chat(ChatRequest request, CancellationToken cancellationToken)
        {
            var command = new SendMessageCommand(request);
            var response = await _handler.HandleAsync(command, cancellationToken);
            return Ok(response);
        }
    }
}
