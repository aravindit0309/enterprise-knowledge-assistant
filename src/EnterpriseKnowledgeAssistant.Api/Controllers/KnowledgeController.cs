using Microsoft.AspNetCore.Mvc;
using EnterpriseKnowledgeAssistant.Service;

[ApiController]
[Route("api/[controller]")]
public class KnowledgeController : ControllerBase
{
    private readonly IKnowledgeService _service;

    public KnowledgeController(IKnowledgeService service) => _service = service;

    [HttpGet]
    public IActionResult GetAll()
    {
        var items = _service.GetKnowledgeItems();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var item = _service.GetKnowledgeItem(id);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpGet("bedrock")]
    public IActionResult CallBedrock()
    {
        return Ok("Hello from Bedrock!");
    }
}
