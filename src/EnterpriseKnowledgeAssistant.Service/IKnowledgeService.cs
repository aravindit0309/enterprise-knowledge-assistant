using EnterpriseKnowledgeAssistant.Service.Models;

namespace EnterpriseKnowledgeAssistant.Service;

public interface IKnowledgeService
{
    IEnumerable<KnowledgeItem> GetKnowledgeItems();
    KnowledgeItem? GetKnowledgeItem(string id);
}
