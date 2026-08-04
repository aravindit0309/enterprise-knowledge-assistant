# Sequence Diagram

User

↓

POST /api/chat

↓

ChatController

↓

SendMessageCommandHandler

↓

Conversation Repository

↓

AgentOrchestrator

↓

EnterpriseQueryRouter

↓

Need Knowledge?

NO

↓

Nova Lite

↓

Response

OR

YES

↓

SearchKnowledgeBaseTool

↓

Embedding Service

↓

Titan Embeddings

↓

pgvector

↓

Relevant Chunks

↓

Nova Lite

↓

Grounded Response

↓

Persist Conversation

↓

HTTP Response