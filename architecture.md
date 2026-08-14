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

Sprint 8: Web Research Integration
Phase 2 treats web research results as temporary grounding context rather 
than extending the existing enterprise AgentSource contract. A unified evidence/source model will be introduced only 
when multi-source evidence aggregation and attribution require it.