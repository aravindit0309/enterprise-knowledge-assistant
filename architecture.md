# Enterprise Knowledge Assistant

## Overview

Enterprise Knowledge Assistant is a production-style AI application built using .NET 10 and AWS.

The project demonstrates:

- Clean Architecture
- CQRS using MediatR
- Retrieval Augmented Generation (RAG)
- Amazon Bedrock integration
- Semantic Search
- Agentic AI
- PostgreSQL + pgvector
- Production-oriented architecture

The objective is to demonstrate Software Architect / Solution Architect level design rather than only LLM integration.

---

## Technology Stack

Backend

- .NET 10
- ASP.NET Core Web API
- C#
- Clean Architecture
- CQRS (MediatR)

AI

- Amazon Bedrock
- Amazon Nova Lite
- Amazon Titan Text Embeddings V2

Database

- PostgreSQL
- pgvector

Document Processing

- PdfPig
- OpenXML SDK

Infrastructure

- Docker
- AWS SDK

---

## High Level Architecture

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

---

## Layer Responsibilities

### API

- Controllers
- Dependency Injection
- HTTP Endpoints

### Application

- CQRS
- MediatR
- Use Cases
- Interfaces

### Domain

- Business entities
- Enums
- Domain logic

### Infrastructure

- AWS
- PostgreSQL
- Embeddings
- Semantic Search
- Agent Framework

---

## AI Components

- Chat Service
- Embedding Service
- Semantic Search
- Agent Decision Service
- Agent Orchestrator
- Knowledge Search Tool

---

## Design Principles

- Dependency Inversion
- Clean Architecture
- Single Responsibility
- Low AWS Cost
- Extensible Agent Framework