# Agent Flow

## Overview

Sprint 5 introduces Agentic AI.

Instead of always performing semantic search, an AI agent determines whether a tool is required before answering.

---

## Request Flow

User

↓

ChatController

↓

SendMessageCommandHandler

↓

AgentOrchestrator

↓

EnterpriseQueryRouter

↓

Enterprise Question?

YES

↓

SearchKnowledgeBaseTool

↓

Semantic Search

↓

Nova Lite

↓

Grounded Answer

NO

↓

AgentDecisionService

↓

Requires Tool?

YES

↓

Selected Tool

↓

Generate Response

NO

↓

Direct Chat

↓

Nova Lite

↓

Response

---

## Agent Components

### AgentOrchestrator

Coordinates the complete AI workflow.

Responsibilities

- Request orchestration
- Tool selection
- Tool execution
- Response generation

---

### AgentDecisionService

Uses Nova Lite to decide whether a tool is required.

Returns

- RequiresTool
- ToolName
- ToolInput

---

### EnterpriseQueryRouter

Performs fast deterministic routing for obvious enterprise questions.

Benefits

- Lower latency
- Lower AWS cost
- Improved routing consistency

---

### Agent Tools

Current

- SearchKnowledgeBaseTool

Future

- SQL Tool
- Ticket Tool
- Email Tool