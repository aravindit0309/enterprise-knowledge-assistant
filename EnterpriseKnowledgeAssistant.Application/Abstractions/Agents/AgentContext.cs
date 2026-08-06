namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public sealed class AgentContext
    {
        private readonly Dictionary<string, object> _items = new();

        public IReadOnlyDictionary<string, object> Items => _items;

        public void Set<T>(string key, T value)
        {
            _items[key] = value!;
        }

        public T? Get<T>(string key)
        {
            if (_items.TryGetValue(key, out var value) && value is T typedValue)
            {
                return typedValue;
            }

            return default;
        }
    }
}
