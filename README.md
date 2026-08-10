# Enterprise Knowledge Assistant

A production-style AI-powered Enterprise Knowledge Assistant built with **.NET 10**, **AWS Bedrock**, **PostgreSQL**, and **pgvector**.

The project demonstrates enterprise application architecture, Retrieval-Augmented Generation (RAG), semantic search, and Agentic AI using Clean Architecture principles.

---

# Project Objectives

The goal of this project is to build a portfolio-quality AI application that demonstrates skills expected from a:

- Software Architect
- Solution Architect
- Principal Engineer
- Staff Engineer

The focus is on:

- Clean Architecture
- Enterprise design patterns
- Large Language Model (LLM) integration
- Retrieval-Augmented Generation (RAG)
- Semantic Search
- Agentic AI
- AWS Cloud Integration
- Production-oriented application design

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

# Sprint Progress

| Sprint   | Status | Features |
| -------- | ------ | -------- |
| Sprint 1 | ✅ | Clean Architecture, CQRS, Dependency Injection |
| Sprint 2 | ✅ | Amazon Bedrock Chat Integration |
| Sprint 3 | ✅ | Conversation Persistence |
| Sprint 4 | ✅ | RAG, Embeddings, Semantic Search |
| Sprint 5 | ✅ | Agentic AI, Tool Routing, Enterprise Query Router |
| Sprint 6 | ✅ | AI Planning Agent, Execution Plans, Supervisor Agent, Multi-step Agent Execution |
| Sprint 7 | ✅ | Memory Agent, SQL Agent, Tool-based Agent Execution |


# Cost Optimizations

This project was intentionally designed to minimize AWS costs.

Approach:

- Amazon Nova Lite for chat
- Titan Text Embeddings V2
- Local PostgreSQL
- Local pgvector
- Local document storage
- On-demand Bedrock API calls

No dedicated AI infrastructure runs continuously during development.


# Repository Structure

```

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

# Learning Outcomes

This project demonstrates practical experience with:

- Enterprise Architecture
- Large Language Models (LLMs)
- Amazon Bedrock
- Retrieval-Augmented Generation (RAG)
- Semantic Search
- Agentic AI
- PostgreSQL Vector Search
- Clean Architecture
- CQRS
- AWS Integration

---

# License

This project is intended for learning and portfolio purposes.