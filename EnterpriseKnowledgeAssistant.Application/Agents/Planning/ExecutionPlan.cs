namespace EnterpriseKnowledgeAssistant.Application.Agents.Planning
{
    public sealed class ExecutionPlan
    {
        public IList<ExecutionStep> Steps { get; } = new List<ExecutionStep>();
    }
}
