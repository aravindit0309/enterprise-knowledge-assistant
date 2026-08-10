# Sequence Diagram

# Sequence Diagrams

This document describes the main runtime flows of the Enterprise Knowledge Assistant.

---

## 1. General Chat

A general conversation does not require enterprise knowledge or agent tools.

```mermaid
sequenceDiagram
    participant User
    participant API as ChatController
    participant Handler as SendMessageCommandHandler
    participant Supervisor as SupervisorAgent
    participant Planner as AI Planner
    participant Orchestrator as AgentOrchestrator
    participant Nova as Amazon Nova Lite

    User->>API: Send message
    API->>Handler: SendMessageCommand
    Handler->>Supervisor: Process request
    Supervisor->>Planner: Generate execution plan
    Planner-->>Supervisor: Respond
    Supervisor->>Orchestrator: Execute plan
    Orchestrator->>Nova: Generate response
    Nova-->>Orchestrator: Response
    Orchestrator-->>Supervisor: AgentResult
    Supervisor-->>Handler: AgentResult
    Handler-->>API: ChatResponse
    API-->>User: Response
```

---

## 2. RAG / Enterprise Knowledge Query

For questions requiring enterprise knowledge, the planner generates a `Retrieve → Respond` execution plan.

```mermaid
sequenceDiagram
    participant User
    participant API as ChatController
    participant Handler as SendMessageCommandHandler
    participant Supervisor as SupervisorAgent
    participant Planner as AI Planner
    participant Orchestrator as AgentOrchestrator
    participant Tool as SearchKnowledgeBaseTool
    participant Search as SemanticSearchCommandHandler
    participant Embedding as Titan Embeddings V2
    participant DB as PostgreSQL + pgvector
    participant Nova as Amazon Nova Lite

    User->>API: Ask enterprise knowledge question
    API->>Handler: SendMessageCommand
    Handler->>Supervisor: Process request

    Supervisor->>Planner: Generate execution plan
    Planner-->>Supervisor: Retrieve → Respond

    Supervisor->>Orchestrator: Execute plan

    Orchestrator->>Tool: Execute search
    Tool->>Search: SemanticSearchCommand
    Search->>Embedding: Generate query embedding
    Embedding-->>Search: Query embedding

    Search->>DB: Semantic vector search
    DB-->>Search: Top matching chunks
    Search-->>Tool: Search results
    Tool-->>Orchestrator: Knowledge + Sources

    Orchestrator->>Nova: Conversation + retrieved context
    Nova-->>Orchestrator: Grounded response

    Orchestrator-->>Supervisor: AgentResult
    Supervisor-->>Handler: AgentResult
    Handler-->>API: Response + Sources
    API-->>User: Grounded answer
```

---

## 3. Memory Agent — Store Memory

The planner can identify information that should be stored as long-term memory.

```mermaid
sequenceDiagram
    participant User
    participant API as ChatController
    participant Handler as SendMessageCommandHandler
    participant Supervisor as SupervisorAgent
    participant Planner as AI Planner
    participant Orchestrator as AgentOrchestrator
    participant Tool as StoreMemoryTool
    participant Embedding as Titan Embeddings V2
    participant DB as PostgreSQL + pgvector
    participant Nova as Amazon Nova Lite

    User->>API: Remember my preferred database is PostgreSQL
    API->>Handler: SendMessageCommand
    Handler->>Supervisor: Process request

    Supervisor->>Planner: Generate execution plan
    Planner-->>Supervisor: StoreMemory → Respond

    Supervisor->>Orchestrator: Execute plan

    Orchestrator->>Tool: Store memory
    Tool->>Embedding: Generate embedding
    Embedding-->>Tool: Memory embedding

    Tool->>DB: Store memory + embedding
    DB-->>Tool: Memory stored

    Tool-->>Orchestrator: Memory stored

    Orchestrator->>Nova: Generate response
    Nova-->>Orchestrator: Confirmation

    Orchestrator-->>Supervisor: AgentResult
    Supervisor-->>Handler: AgentResult
    Handler-->>API: Response
    API-->>User: Confirmation
```

---

## 4. Memory Agent — Search Memory

The planner can search previously stored memories when the user asks about remembered information.

```mermaid
sequenceDiagram
    participant User
    participant API as ChatController
    participant Handler as SendMessageCommandHandler
    participant Supervisor as SupervisorAgent
    participant Planner as AI Planner
    participant Orchestrator as AgentOrchestrator
    participant Tool as SearchMemoryTool
    participant Embedding as Titan Embeddings V2
    participant DB as PostgreSQL + pgvector
    participant Nova as Amazon Nova Lite

    User->>API: What database do I prefer?
    API->>Handler: SendMessageCommand
    Handler->>Supervisor: Process request

    Supervisor->>Planner: Generate execution plan
    Planner-->>Supervisor: SearchMemory → Respond

    Supervisor->>Orchestrator: Execute plan

    Orchestrator->>Tool: Search memory
    Tool->>Embedding: Generate query embedding
    Embedding-->>Tool: Query embedding

    Tool->>DB: Semantic memory search
    DB-->>Tool: Matching memory records

    Tool-->>Orchestrator: Memory context

    Orchestrator->>Nova: Conversation + memory context
    Nova-->>Orchestrator: Response

    Orchestrator-->>Supervisor: AgentResult
    Supervisor-->>Handler: AgentResult
    Handler-->>API: Response
    API-->>User: Answer
```

---

## 5. SQL Agent

The SQL Agent handles questions that require querying structured enterprise data.

The planner passes a natural-language request to the SQL tool. The SQL tool is responsible for SQL generation, validation, and execution.

```mermaid
sequenceDiagram
    participant User
    participant API as ChatController
    participant Handler as SendMessageCommandHandler
    participant Supervisor as SupervisorAgent
    participant Planner as AI Planner
    participant Orchestrator as AgentOrchestrator
    participant Tool as ExecuteSqlTool
    participant Generator as SQL Generator
    participant Nova as Amazon Nova Lite
    participant Validator as SQL Validator
    participant DB as PostgreSQL

    User->>API: How many documents have been uploaded?
    API->>Handler: SendMessageCommand
    Handler->>Supervisor: Process request

    Supervisor->>Planner: Generate execution plan
    Planner-->>Supervisor: ExecuteSql → Respond

    Supervisor->>Orchestrator: Execute plan

    Orchestrator->>Tool: Execute natural-language request

    Tool->>Generator: Generate SQL
    Generator->>Nova: Generate read-only PostgreSQL query
    Nova-->>Generator: SELECT COUNT(*) FROM "Documents"

    Generator-->>Tool: SQL query

    Tool->>Validator: Validate SQL
    Validator-->>Tool: Valid read-only query

    Tool->>DB: Execute SQL
    DB-->>Tool: Query result

    Tool-->>Orchestrator: SQL result

    Orchestrator->>Nova: Conversation + SQL result
    Nova-->>Orchestrator: 9 documents have been uploaded

    Orchestrator-->>Supervisor: AgentResult
    Supervisor-->>Handler: AgentResult
    Handler-->>API: Response
    API-->>User: Answer
```

---

## 6. Agent Planning and Execution

This diagram shows the core agent architecture introduced in Sprint 6 and extended in Sprint 7.

The planner decides **what to do**, while the orchestrator executes the generated plan.

```mermaid
sequenceDiagram
    participant User
    participant Supervisor as SupervisorAgent
    participant Planner as AI Planner
    participant Plan as ExecutionPlan
    participant Orchestrator as AgentOrchestrator
    participant Tools as Agent Tools
    participant Nova as Amazon Nova Lite

    User->>Supervisor: User request

    Supervisor->>Planner: Understand request
    Planner->>Nova: Generate execution plan
    Nova-->>Planner: Plan JSON

    Planner-->>Supervisor: ExecutionPlan
    Supervisor->>Plan: Validate / map plan

    Supervisor->>Orchestrator: Execute plan

    loop For each execution step
        Orchestrator->>Tools: Execute tool
        Tools-->>Orchestrator: AgentToolResult
    end

    Orchestrator->>Nova: Conversation + execution context
    Nova-->>Orchestrator: Final response

    Orchestrator-->>Supervisor: AgentResult
    Supervisor-->>User: Final response
```

---

## 7. Multi-Step Agent Execution

The planner can generate multiple execution steps when a request requires more than one capability.

```mermaid
sequenceDiagram
    participant User
    participant Planner as AI Planner
    participant Orchestrator as AgentOrchestrator
    participant KB as SearchKnowledgeBaseTool
    participant Memory as SearchMemoryTool
    participant Nova as Amazon Nova Lite

    User->>Planner: Complex user request

    Planner-->>Orchestrator: ExecutionPlan

    Note over Orchestrator: Execute steps in order

    Orchestrator->>KB: Execute Retrieve
    KB-->>Orchestrator: Knowledge context

    Orchestrator->>Memory: Execute SearchMemory
    Memory-->>Orchestrator: Memory context

    Orchestrator->>Nova: Conversation + execution context
    Nova-->>Orchestrator: Final response

    Orchestrator-->>User: Response
```

---

## Architectural Principle

The agent runtime follows a clear separation of responsibilities.

| Component | Responsibility |
|---|---|
| SupervisorAgent | Coordinates the agent workflow |
| AI Planner | Understands intent and creates an execution plan |
| ExecutionPlan | Represents the planner's requested actions |
| AgentOrchestrator | Executes the generated plan |
| Agent Tools | Perform specific capabilities |
| Amazon Nova Lite | Performs AI planning and response generation |
| PostgreSQL + pgvector | Persistent storage and semantic search |

### Core Design Principle

**The Planner decides what to do.**

**The Orchestrator decides how to execute the plan.**

New capabilities can be introduced as agent tools without changing the core execution model.