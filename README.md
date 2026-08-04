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

---

# Solution Architecture

```

Client

↓

ASP.NET Core API

↓

Application Layer

↓

Domain Layer

↓

Infrastructure Layer

├── Amazon Bedrock
├── PostgreSQL
├── pgvector
└── Local File Storage

```

The application follows **Clean Architecture**, ensuring the Application and Domain layers remain independent of infrastructure concerns.

---

# Key Features

## Multi-turn Conversations

- Persistent conversations
- Conversation history
- Context-aware responses

---

## Retrieval-Augmented Generation (RAG)

- Document upload
- PDF
- DOCX
- TXT

Document ingestion includes:

- Text extraction
- Chunking
- Embedding generation
- Vector storage

---

## Semantic Search

- Amazon Titan Embeddings V2
- PostgreSQL pgvector
- Cosine similarity search

The application retrieves the most relevant document chunks before generating responses.

---

## Agentic AI

Sprint 5 introduces an AI Agent layer.

Instead of always performing semantic search, the application decides whether a tool is required before answering.

Current tools:

- Search Knowledge Base

Architecture:

```

User

↓

Agent Orchestrator

↓

Enterprise Query Router

↓

Need Knowledge?

YES

↓

Knowledge Search Tool

↓

Semantic Search

↓

Nova Lite

↓

Grounded Response

NO

↓

Direct Chat

↓

Nova Lite

↓

Response

```

---

# Implemented Architecture

## API Layer

Responsibilities

- REST endpoints
- Request validation
- Dependency Injection

---

## Application Layer

Responsibilities

- CQRS
- MediatR
- Use Cases
- Interfaces
- Agent contracts

---

## Domain Layer

Responsibilities

- Entities
- Business rules
- Domain models

---

## Infrastructure Layer

Responsibilities

- Amazon Bedrock
- PostgreSQL
- Embeddings
- Semantic Search
- Agent Framework
- File Storage

---

# Sprint Progress

| Sprint | Status | Features |
|---------|--------|----------|
| Sprint 1 | ✅ | Clean Architecture, CQRS, Dependency Injection |
| Sprint 2 | ✅ | Amazon Bedrock Chat Integration |
| Sprint 3 | ✅ | Conversation Persistence |
| Sprint 4 | ✅ | RAG, Embeddings, Semantic Search |
| Sprint 5 | ✅ | Agentic AI, Tool Routing, Enterprise Query Router |

---

# AI Workflow

```

User Question

↓

Conversation Loaded

↓

Agent Orchestrator

↓

Enterprise Query Router

↓

Agent Decision

↓

Tool Execution

↓

Semantic Search

↓

Amazon Nova Lite

↓

Grounded Response

↓

Conversation Persisted

↓

API Response

```

---

# Agent Components

## Agent Orchestrator

Coordinates the complete AI workflow.

Responsibilities

- Tool selection
- Tool execution
- Response generation

---

## Enterprise Query Router

Performs deterministic routing for enterprise-related questions.

Benefits

- Lower latency
- Lower AWS cost
- Faster routing
- Reduced LLM calls

---

## Agent Decision Service

Uses Amazon Nova Lite to determine whether a tool is required for non-obvious requests.

---

## Search Knowledge Base Tool

Provides enterprise document search using semantic search.

Workflow

Question

↓

Embedding

↓

pgvector

↓

Relevant Chunks

↓

Grounded Response

---

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

---

# Future Roadmap

## Sprint 6

- JWT Authentication
- Authorization
- Multi-user support

## Sprint 7

- Background document ingestion
- Amazon S3
- OCR

## Sprint 8

Additional AI Tools

- SQL Tool
- Email Tool
- REST API Tool
- Calendar Tool

## Sprint 9

Production Readiness

- CloudWatch Logging
- Health Checks
- Metrics
- Docker Deployment

---

# Design Principles

- Clean Architecture
- SOLID Principles
- Dependency Inversion
- Separation of Concerns
- Extensible Agent Framework
- Low-cost Cloud Architecture
- Production-oriented Design

---

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