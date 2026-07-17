using EnterpriseKnowledgeAssistant.Service.Models;

namespace EnterpriseKnowledgeAssistant.Service;

public class KnowledgeService : IKnowledgeService
{
    private readonly List<KnowledgeItem> _items = new()
    {
        new KnowledgeItem { Id = "1", Title = "Getting Started", Content = "Welcome to EnterpriseKnowledgeAssistant." },
        new KnowledgeItem { Id = "2", Title = "Architecture", Content = "Service-oriented architecture guidance." }
    };

    public IEnumerable<KnowledgeItem> GetKnowledgeItems() => _items;

    public KnowledgeItem? GetKnowledgeItem(string id) => _items.FirstOrDefault(x => x.Id == id);
}
