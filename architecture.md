Client
  ↓
ASP.NET Core API
  ↓
Application
  ↓
Supervisor Agent
  ↓
AI Planner
  ↓
Execution Plan
  ↓
Agent Orchestrator
  ↓
┌───────────────┬───────────────┬───────────────┐
│ RAG           │ Memory        │ SQL           │
│               │               │               │
│ Search KB     │ Store Memory  │ Generate SQL  │
│               │ Search Memory │ Validate SQL  │
│               │               │ Execute SQL   │
└───────────────┴───────────────┴───────────────┘
                    ↓
                 Respond
                    ↓
              Amazon Nova Lite