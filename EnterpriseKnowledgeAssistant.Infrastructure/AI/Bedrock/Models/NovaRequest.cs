using EnterpriseKnowledgeAssistant.Infrastructure.AI.Models;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock.Models
{
    public class NovaRequest
    {
        public string SchemaVersion { get; set; } = "messages-v1";
        //System messages sent by the user will be added to this field and will be used to generate system messages in the response.
        //if we send system messages in the Messages field, they will be creating error at the Nova model invocation.
        public List<TextContent>? System { get; set; } 
        public List<NovaMessage> Messages { get; set; } = new();
        public InferenceConfig InferenceConfig { get; set; } = new();
    }
}
