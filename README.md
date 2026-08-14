# Enterprise Knowledge Assistant

> **Project Status: Sprint #9 — Active Development 🚧**

A production-style AI-powered Enterprise Knowledge Assistant built with **.NET 10**, **AWS Bedrock**, **PostgreSQL**, and **pgvector**.

This project is actively being developed as a hands-on implementation of **production-oriented Generative AI and Agentic AI architecture**. Capabilities are introduced incrementally through development sprints, with the architecture evolving as new AI patterns and engineering considerations are explored.

The current focus is on **advanced agent orchestration, tool integration, research workflows, and grounded AI responses**.

---

## Why This Project?

The goal is to build a AI application that demonstrates the architectural and engineering skills expected from a:

- Software Architect
- Solution Architect
- Principal Engineer
- Staff Engineer

Rather than implementing a simple LLM chatbot, the project explores how multiple AI capabilities can be combined into an extensible application architecture.

The implementation progressively evolves from direct LLM integration to **RAG, Agentic AI, planning, tool orchestration, multi-step execution, memory, SQL interaction, external web research, and grounded synthesis**.

---

## Key Capabilities

- Large Language Model (LLM) integration using Amazon Bedrock
- Retrieval-Augmented Generation (RAG)
- Embeddings and semantic search
- PostgreSQL vector search using pgvector
- Multi-turn conversation persistence
- Agentic AI
- Tool routing and dynamic tool selection
- AI planning and execution plans
- Supervisor-based agent orchestration
- Multi-step agent execution
- Memory agent
- SQL agent
- External web research
- Orchestration across internal RAG, memory, SQL, and external web search
- Grounded AI responses and research workflows
- Clean Architecture and CQRS

> **Note:** The project is still under active development. Some capabilities and architectural components may continue to evolve as new sprints are completed.

---

# Technology Stack

## Backend

- .NET 10
- ASP.NET Core Web API
- C#
- Clean Architecture
- CQRS (MediatR)
- Dependency Injection
- Entity Framework Core

## Artificial Intelligence

- Amazon Bedrock
- Amazon Nova Lite
- Amazon Titan Text Embeddings V2

## Database

- PostgreSQL
- pgvector

## Document Processing

- PdfPig
- Open XML SDK

## Infrastructure

- Docker
- AWS SDK for .NET

---

# Architecture

The application follows **Clean Architecture**, separating the API, application/business logic, domain model, and infrastructure integrations.

At a high level:

```text
Client
   |
   v
API
   |
   v
Application / Orchestration
   |
   +---- Agent Planning & Routing
   |
   +---- Tool Execution
   |       |
   |       +---- RAG / Semantic Search
   |       +---- Memory
   |       +---- SQL
   |       +---- External Web Research
   |
   v
Domain
   |
   v
Infrastructure
   |
   +---- PostgreSQL / pgvector
   +---- AWS Bedrock
   +---- Document Processing
```

Detailed architecture and execution flows are available under the `docs/` directory.

---

# Sprint Progress

| Sprint | Status | Features |
| --- | --- | --- |
| Sprint 1 | ✅ | Clean Architecture, CQRS, Dependency Injection |
| Sprint 2 | ✅ | Amazon Bedrock Chat Integration |
| Sprint 3 | ✅ | Conversation Persistence |
| Sprint 4 | ✅ | RAG, Embeddings, Semantic Search |
| Sprint 5 | ✅ | Agentic AI, Tool Routing, Enterprise Query Router |
| Sprint 6 | ✅ | AI Planning Agent, Execution Plans, Supervisor Agent, Multi-step Agent Execution |
| Sprint 7 | ✅ | Memory Agent, SQL Agent, Tool-based Agent Execution |
| Sprint 8 | ✅ | WebResearch Agent and orchestration across internal RAG, Memory, SQL, and external web search |
| Sprint 9 | 🚧 | Active development — advanced agent orchestration, tool integration, research workflows, and grounded AI responses |

---

# Evolution of the AI Architecture

The project is intentionally being developed in stages to demonstrate how an AI application can evolve beyond a basic chatbot.

```text
LLM Integration
      ↓
Conversation Memory
      ↓
RAG + Embeddings + Semantic Search
      ↓
Agentic AI + Tool Routing
      ↓
Planning + Multi-step Execution
      ↓
Memory + SQL Agents
      ↓
External Web Research
      ↓
Multi-source Orchestration
      ↓
Grounded AI Responses
      ↓
Advanced Agent Architecture
```

This progression is intended to demonstrate not only the use of AI services, but also the **architectural decisions required to orchestrate multiple AI capabilities within a maintainable application**.

---

# Cost Optimization

The project is intentionally designed to minimize AWS costs during development without compromising the architectural goals of the solution.

Approach:

- Amazon Nova Lite for chat
- Amazon Titan Text Embeddings V2
- Local PostgreSQL
- Local pgvector
- Local document storage
- On-demand Bedrock API calls

No dedicated AI infrastructure runs continuously during development.

---

# Repository Structure

```text
src/
├── EnterpriseKnowledgeAssistant.Api
├── EnterpriseKnowledgeAssistant.Application
├── EnterpriseKnowledgeAssistant.Domain
└── EnterpriseKnowledgeAssistant.Infrastructure

docs/
├── Architecture.md
├── AgentFlow.md
├── SequenceDiagram.md
└── FutureRoadmap.md
```

---

# Documentation

The repository contains additional technical documentation covering:

- Architecture
- Agent execution flow
- Sequence diagrams
- Future roadmap

See the `docs/` directory for details.

---

# Learning Outcomes

This project provides hands-on experience with:

- Enterprise Architecture
- Generative AI
- Large Language Models (LLMs)
- Amazon Bedrock
- Retrieval-Augmented Generation (RAG)
- Embeddings
- Semantic Search
- Agentic AI
- Agent orchestration
- Tool-based AI execution
- PostgreSQL Vector Search
- Clean Architecture
- CQRS
- AWS Integration
- AI application design

---

# Project Status

**Sprint 9 — Active Development 🚧**

This repository represents an evolving AI architecture project rather than a completed product. New capabilities are being implemented incrementally, with an emphasis on understanding and demonstrating production-oriented patterns for **Generative AI, RAG, Agentic AI, orchestration, and grounded AI systems**.

---

# License

This project is intended for learning and portfolio purposes.
